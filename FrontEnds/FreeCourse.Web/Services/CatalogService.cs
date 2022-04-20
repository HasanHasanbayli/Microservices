using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _httpClient;

    public CatalogService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CourseViewModel>> GetAllCourseAsync()
    {
        var response = await _httpClient.GetAsync("api/Courses");

        if (response.IsSuccessStatusCode)
        {
            return null;
        }

        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

        return responseSuccess.Data;
    }

    public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
    {
        var response = await _httpClient.GetAsync("api/Categories");

        if (response.IsSuccessStatusCode)
        {
            return null;
        }

        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();

        return responseSuccess.Data;
    }

    public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
    {
        var response = await _httpClient.GetAsync($"api/Courses/GetAllByUserId/{userId}");

        if (response.IsSuccessStatusCode)
        {
            return null;
        }

        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

        return responseSuccess.Data;
    }

    public async Task<CourseViewModel> GetCourseById(string courseId)
    {
        var response = await _httpClient.GetAsync($"api/Courses/{courseId}");

        if (response.IsSuccessStatusCode)
        {
            return null;
        }

        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();

        return responseSuccess.Data;
    }

    public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
    {
        var response = await _httpClient.PostAsJsonAsync("courses", courseCreateInput);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
    {
        var response = await _httpClient.PutAsJsonAsync("courses", courseUpdateInput);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCourseAsync(string courseId)
    {
        var response = await _httpClient.DeleteAsync($"api/Courses/{courseId}");

        return response.IsSuccessStatusCode;
    }
}