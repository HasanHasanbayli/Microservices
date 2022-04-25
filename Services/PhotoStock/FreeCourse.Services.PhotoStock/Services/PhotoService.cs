using FreeCourse.Services.PhotoStock.DTOs;
using FreeCourse.Shared.DTOs;

namespace FreeCourse.Services.PhotoStock.Services;

public class PhotoService : IPhotoService
{
    public async Task<Response<PhotoDto>> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
    {
        if (photo == null || photo.Length <= 0)
            return Response<PhotoDto>.Fail("photo is empty", 400);

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

        await using var stream = new FileStream(path, FileMode.Create);

        await photo.CopyToAsync(stream, cancellationToken);

        var returnPath = photo.FileName;

        PhotoDto photoDto = new()
        {
            Url = returnPath
        };

        return Response<PhotoDto>.Success(photoDto, 200);
    }

    public async Task<Response<NoContent>> PhotoDelete(string photoUrl)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);

        if (!File.Exists(path))
            return await Task.FromResult(Response<NoContent>.Fail("photo not found", 404));

        File.Delete(path);

        return await Task.FromResult(Response<NoContent>.Success(204));
    }
}