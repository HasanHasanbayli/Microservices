using FreeCourse.Services.PhotoStock.Services;
using FreeCourse.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.PhotoStock.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhotosController : CustomBaseController
{
    private readonly IPhotoService _photoService;

    public PhotosController(IPhotoService photoService)
    {
        _photoService = photoService;
    }

    [HttpPost]
    public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
    {
        var response = await _photoService.PhotoSave(photo, cancellationToken);

        return CreateActionResultInstance(response);
    }

    [HttpDelete]
    public async Task<IActionResult> PhotoDelete(string photoUrl)
    {
        var response = await _photoService.PhotoDelete(photoUrl);

        return CreateActionResultInstance(response);
    }
}