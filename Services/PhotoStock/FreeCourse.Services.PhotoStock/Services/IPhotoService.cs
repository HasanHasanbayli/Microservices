using FreeCourse.Services.PhotoStock.DTOs;
using FreeCourse.Shared.DTOs;

namespace FreeCourse.Services.PhotoStock.Services;

public interface IPhotoService
{
    Task<Response<PhotoDto>> PhotoSave(IFormFile photo, CancellationToken cancellationToken);
    Task<Response<NoContent>> PhotoDelete(string photoUrl);
}