using System;
using api.DTOs;
using web.api.Models;

namespace web.api.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();
    Task<CategoryDTO> GetCategoryAsync(Guid id);
    Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category);
    Task<CategoryDTO> UpdateCategoryAsync(Guid id, CategoryDTO category);

    Task<bool> DeleteCategoryAsync(Guid id);

    Task Activate(Guid id);

    Task Deactivate(Guid id);
}
