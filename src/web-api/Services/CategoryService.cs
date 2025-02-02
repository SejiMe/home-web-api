using System.Security.Cryptography.X509Certificates;
using api.DTOs;
using web.api.Infrastructure.Data;
using web.api.Interfaces;
using web.api.Models;

namespace web.api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HomeDbContext _context;

        public CategoryService(HomeDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO dto)
        {
            // Must check name first before creating a new category
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(d =>
                d.Name == dto.Name
            );

            if (existingCategory != null)
                throw new InvalidOperationException("Category already exists");

            await _context.Categories.AddAsync(
                new Category()
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    TimeStamp = DateTime.UtcNow,
                }
            );

            await _context.SaveChangesAsync();

            return new CategoryDTO()
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
            };
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is null)
                return false;
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CategoryDTO> GetCategoryAsync(Guid id)
        {
            var res =
                await _context.Categories.FindAsync(id)
                ?? throw new NullReferenceException("Category not found");

            return new CategoryDTO()
            {
                Id = res.Id,
                Name = res.Name,
                Description = res.Description,
            };
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
        {
            return await _context
                .Categories.Select(c => new CategoryDTO()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                })
                .ToListAsync();
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(Guid id, CategoryDTO dto)
        {
            var existingCategory =
                await _context.Categories.FindAsync(id)
                ?? throw new NullReferenceException("Category not found");
            ;

            existingCategory.Name = dto.Name;
            existingCategory.Description = dto.Description;
            existingCategory.TimeStamp = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new CategoryDTO()
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                Description = existingCategory.Description,
            };
        }

        public Task Activate(Guid id)
        {
            var category =
                _context.Categories.Find(id)
                ?? throw new NullReferenceException("Category not found");

            category.ActivateCategory();
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task Deactivate(Guid id)
        {
            var category =
                _context.Categories.Find(id)
                ?? throw new NullReferenceException("Category not found");

            category.DeactiveCategory();
            _context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
