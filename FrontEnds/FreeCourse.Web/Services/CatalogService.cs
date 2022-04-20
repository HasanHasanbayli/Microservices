using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services;

public class CatalogService:ICatalogService
{
    private readonly HttpClient _httpClient;

    public CatalogService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<List<CourseViewModel>> GetAllCourseAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<CategoryViewModel>> GetAllCategoryAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<CourseViewModel> GetCourseById(string courseId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteCourseAsync(string courseId)
    {
        throw new NotImplementedException();
    }
}