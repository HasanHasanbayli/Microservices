using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Models.PhotoStock;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services;

public class PhotoStockService : IPhotoStockService
{
    private readonly HttpClient _httpClient;

    public PhotoStockService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PhotoViewModel?> UploadPhoto(IFormFile photo)
    {
        if (photo == null || photo.Length <= 0) return null;

        var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";

        using var ms = new MemoryStream();

        await photo.CopyToAsync(ms);

        var multipartFormDataContent = new MultipartFormDataContent();

        multipartFormDataContent.Add(new ByteArrayContent(ms.ToArray()), "photo", randomFileName);

        var response = await _httpClient.PostAsync("photos", multipartFormDataContent);

        if (!response.IsSuccessStatusCode) return null;

        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();

        return responseSuccess.Data;
    }

    public async Task<bool> DeletePhoto(string photoUrl)
    {
        var response = await _httpClient.DeleteAsync($"photos?photoUrl={photoUrl}");

        return response.IsSuccessStatusCode;
    }
}