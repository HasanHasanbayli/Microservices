using AutoMapper;
using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.DTOs;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services;

public class CategoryService : ICategoryService
{
    private readonly IMongoCollection<Category> _categoryCollection;
    private readonly IMapper _mapper;

    public CategoryService(IMapper mapper, IDataBaseSettings dataBaseSettings)
    {
        var client = new MongoClient(dataBaseSettings.ConnectionString);
        var database = client.GetDatabase(dataBaseSettings.DatabaseName);
        _categoryCollection = database.GetCollection<Category>(dataBaseSettings.CategoryCollectionName);
        _mapper = mapper;
    }

    public async Task<Response<List<CategoryDTO>>> GetAllAsync()
    {
        var categories = await _categoryCollection.Find(category => true).ToListAsync();

        return Response<List<CategoryDTO>>.Success(_mapper.Map<List<CategoryDTO>>(categories), 200);
    }

    public async Task<Response<CategoryDTO>> CreateAsync(CategoryCreateDTO categoryCreateDto)
    {
        var category = _mapper.Map<Category>(categoryCreateDto);

        await _categoryCollection.InsertOneAsync(category);

        return Response<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(category), 200);
    }

    public async Task<Response<CategoryDTO>> GetByIdAsync(string id)
    {
        var category = await _categoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        return category == null
            ? Response<CategoryDTO>.Fail("Category not found", 404)
            : Response<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(category), 200);
    }
}