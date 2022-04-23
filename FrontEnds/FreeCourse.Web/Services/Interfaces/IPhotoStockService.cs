using FreeCourse.Web.Models.PhotoStock;

namespace FreeCourse.Web.Services.Interfaces;

public interface IPhotoStockService
{
    Task<PhotoViewModel?> UploadPhoto(IFormFile photo);
    Task<bool> DeletePhoto(string photoUrl);
}