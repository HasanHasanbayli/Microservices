using AutoMapper;
using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.DTOs;
using FreeCourse.Shared.Messages;
using MassTransit;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services;

public class CourseService : ICourseService
{
    private readonly IMongoCollection<Category> _categoryCollection;
    private readonly IMongoCollection<Course> _courseCollection;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public CourseService(IMapper mapper, IDataBaseSettings dataBaseSettings, IPublishEndpoint publishEndpoint)
    {
        var client = new MongoClient(dataBaseSettings.ConnectionString);
        var database = client.GetDatabase(dataBaseSettings.DatabaseName);
        _courseCollection = database.GetCollection<Course>(dataBaseSettings.CourseCollectionName);
        _categoryCollection = database.GetCollection<Category>(dataBaseSettings.CategoryCollectionName);
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Shared.DTOs.Response<List<CourseDTO>>> GetAllAsync()
    {
        var courses = await _courseCollection.Find(course => true).ToListAsync();

        if (courses.Any())
            foreach (var course in courses)
                course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
        else
            courses = new List<Course>();

        return Shared.DTOs.Response<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(courses), 200);
    }

    public async Task<Shared.DTOs.Response<CourseDTO>> GetByIdAsync(string id)
    {
        var course = await _courseCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        if (course == null) return Shared.DTOs.Response<CourseDTO>.Fail("Course not found", 404);

        course.Category =
            await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstOrDefaultAsync();

        return Shared.DTOs.Response<CourseDTO>.Success(_mapper.Map<CourseDTO>(course), 200);
    }

    public async Task<Shared.DTOs.Response<List<CourseDTO>>> GetAllByUserId(string userId)
    {
        var courses = await _courseCollection.Find(x => x.UserId == userId).ToListAsync();

        if (courses.Any())
            foreach (var course in courses)
                course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
        else
            courses = new List<Course>();

        return Shared.DTOs.Response<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(courses), 200);
    }

    public async Task<Shared.DTOs.Response<CourseDTO>> CreateAsync(CourseCreateDTO courseCreateDto)
    {
        var newCourse = _mapper.Map<Course>(courseCreateDto);

        newCourse.CreatedDate = DateTime.Now;

        await _courseCollection.InsertOneAsync(newCourse);

        return Shared.DTOs.Response<CourseDTO>.Success(_mapper.Map<CourseDTO>(newCourse), 200);
    }

    public async Task<Shared.DTOs.Response<NoContent>> UpdateAsync(CourseUpdateDTO courseUpdateDto)
    {
        var updateCourse = _mapper.Map<Course>(courseUpdateDto);

        var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id, updateCourse);

        if (result == null) return Shared.DTOs.Response<NoContent>.Fail("Course not found", 404);

        await _publishEndpoint.Publish(new CourseNameChangedEvent
        {
            CourseId = updateCourse.Id, UpdatedName = courseUpdateDto.Name
        });

        return Shared.DTOs.Response<NoContent>.Success(204);
    }

    public async Task<Shared.DTOs.Response<NoContent>> DeleteAsync(string id)
    {
        var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);

        return result.DeletedCount > 0
            ? Shared.DTOs.Response<NoContent>.Success(204)
            : Shared.DTOs.Response<NoContent>.Fail("Course not found", 404);
    }
}