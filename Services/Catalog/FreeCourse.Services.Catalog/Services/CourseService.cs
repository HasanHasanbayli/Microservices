using AutoMapper;
using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.DTOs;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services;

internal class CourseService:ICourseService
{
    private readonly IMongoCollection<Course> _courseCollection;
    private readonly IMongoCollection<Category> _categoryCollection;

    private readonly IMapper _mapper;

    public CourseService(IMapper mapper, IDataBaseSettings dataBaseSettings)
    {
        var client = new MongoClient(dataBaseSettings.ConnectionString);

        var database = client.GetDatabase(dataBaseSettings.DatabaseName);

        _courseCollection = database.GetCollection<Course>(dataBaseSettings.CourseCollectionName);

        _categoryCollection = database.GetCollection<Category>(dataBaseSettings.CourseCollectionName);

        _mapper = mapper;
    }

    public async Task<Response<List<CourseDTO>>> GetAllAsync()
    {
        var courses = await _courseCollection.Find(course => true).ToListAsync();

        if (courses.Any())
        {
            foreach (var course in courses)
            {
                course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
            }
        }
        else
        {
            courses = new List<Course>();
        }

        return Response<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(courses), 200);
    }

    public async Task<Response<CourseDTO>> GetByIdAsync(string id)
    {
        var course = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();
        if (course == null)
        {
            return Response<CourseDTO>.Fail("Course not found", 404);
        }

        course.Category =
            await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstOrDefaultAsync();

        return Response<CourseDTO>.Success(_mapper.Map<CourseDTO>(course), 200);
    }

    public async Task<Response<List<CourseDTO>>> GetAllByUserId(string userId)
    {
        var courses = await _courseCollection.Find<Course>(x => x.UserId == userId).ToListAsync();

        if (courses.Any())
        {
            foreach (var course in courses)
            {
                course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
            }
        }
        else
        {
            courses = new List<Course>();
        }

        return Response<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(courses), 200);
    }

    public async Task<Response<CourseDTO>> CreateAsync(CourseCreateDTO courseCreateDto)
    {
        var newCourse = _mapper.Map<Course>(courseCreateDto);

        newCourse.CreatedDate = DateTime.Now;
        await _courseCollection.InsertOneAsync(newCourse);

        return Response<CourseDTO>.Success(_mapper.Map<CourseDTO>(newCourse), 200);
    }

    public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDTO courseUpdateDto)
    {
        var updateCourse = _mapper.Map<Course>(courseUpdateDto);

        var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id, updateCourse);

        if (result == null)
        {
            return Response<NoContent>.Fail("Course not found", 404);
        }

        return Response<NoContent>.Success(204);
    }

    public async Task<Response<NoContent>> DeleteAsync(string id)
    {
        var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);

        if (result.DeletedCount > 0)
        {
            return Response<NoContent>.Success(204);
        }

        return Response<NoContent>.Fail("Course not found", 404);
    }
}