using System;
using api.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using web.api.Interfaces;

namespace web.api.Endpoints;

public static class CategoryEndpoint
{
    public static void RegisterCategoryEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/categories", GetAllCategories);
        builder.MapGet("/categories/{id:guid}", GetCategory);
        builder.MapPost("/categories", PostCategory);
    }

    private static async Task<Results<Ok<IReadOnlyList<CategoryDTO>>, NotFound>> GetAllCategories(
        ICategoryService categoryService
    )
    {
        var categoryList = await categoryService.GetCategoriesAsync();

        if (categoryList == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok<IReadOnlyList<CategoryDTO>>([.. categoryList]);
    }

    private static async Task<Results<Ok<CategoryDTO>, NotFound<Guid>>> GetCategory(
        Guid id,
        ICategoryService categoryService
    )
    {
        var category = await categoryService.GetCategoryAsync(id);

        if (category == null)
        {
            return TypedResults.NotFound(id);
        }

        return TypedResults.Ok(category);
    }

    public record CreateCategoryRequest(string Name, string? Description);

    private static async Task<
        Results<Created<CategoryDTO>, ProblemHttpResult, BadRequest<CreateCategoryRequest>>
    > PostCategory(CreateCategoryRequest request, ICategoryService categoryService)
    {
        try
        {
            var createdCategory = await categoryService.CreateCategoryAsync(
                new CategoryDTO()
                {
                    Id = Guid.CreateVersion7(),
                    Name = request.Name,
                    Description = request.Description,
                }
            );
            return TypedResults.Created("CreateCategory", createdCategory);
        }
        catch (InvalidOperationException exists)
        {
            ProblemDetails details = new ProblemDetails()
            {
                Title = "Category already exists",
                Detail = exists.Message,
                Status = StatusCodes.Status409Conflict,
            };
            return TypedResults.Problem(details);
        }
    }
}
