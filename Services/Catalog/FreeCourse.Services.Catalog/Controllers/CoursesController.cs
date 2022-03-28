using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Catalog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : CustomBaseController
{
    private readonly ICourseService _courseService;

    internal CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _courseService.GetAllAsync();

        return CreateActionResultInstance(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var response = await _courseService.GetByIdAsync(id);

        return CreateActionResultInstance(response);
    }

    [Route("/api[controller]/GetAllByUserId/{userId}")]
    public async Task<IActionResult> GetAllByUserId(string userId)
    {
        var response = await _courseService.GetAllByUserId(userId);

        return CreateActionResultInstance(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseCreateDTO courseCreateDto)
    {
        var response = await _courseService.CreateAsync(courseCreateDto);

        return CreateActionResultInstance(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update(CourseUpdateDTO courseUpdateDto)
    {
        var response = await _courseService.UpdateAsync(courseUpdateDto);

        return CreateActionResultInstance(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Update(string id)
    {
        var response = await _courseService.DeleteAsync(id);

        return CreateActionResultInstance(response);
    }
}