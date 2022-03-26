using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Shared.DTOs;

namespace FreeCourse.Services.Catalog.Services;

interface ICategoryService
{
    Task<Response<List<CategoryDTO>>> GetAllAsync();

    Task<Response<CategoryDTO>> CreateAsync(Category category);

    Task<Response<CategoryDTO>> GetByIdAsync(string id);
}