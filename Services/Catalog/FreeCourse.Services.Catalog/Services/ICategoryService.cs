﻿using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Shared.DTOs;

namespace FreeCourse.Services.Catalog.Services;

public interface ICategoryService
{
    Task<Response<List<CategoryDTO>>> GetAllAsync();

    Task<Response<CategoryDTO>> CreateAsync(CategoryCreateDTO categoryCreateDto);

    Task<Response<CategoryDTO>> GetByIdAsync(string id);
}