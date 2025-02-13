using System;
using api.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using web.api.Interfaces;

namespace web.api.Endpoints;

public static class CategoryEndpoint
{
    public static void RegisterCategoryEndpoint(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("categories").WithTags("Categories Endpoint");
        group.MapGet("", GetAllCategories).RequireAuthorization();
        group.MapGet("{id:guid}", GetCategory);
        group
            .MapPatch(
                "{id:guid}",
                (
                    [FromRoute] Guid id,
                    [FromBody] UpdateCategoryRequest request,
                    ICategoryService categoryService
                ) => UpdateCategory(id, request, categoryService)
            )
            .WithDescription("Update a category")
            .RequireAuthorization();

        group
            .MapPost("", PostCategory)
            .WithDescription("Create a new category")
            .RequireAuthorization();

        group
            .MapDelete(
                "{id:guid}",
                (
                    [FromRoute] Guid id,
                    ICategoryService categoryService,
                    IHttpContextAccessor accessor
                ) => DeleteCategory(id, categoryService, accessor)
            )
            .WithDescription("Delete a category")
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .RequireAuthorization();
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

    private static async Task<Results<Ok<CategoryDTO>, ProblemHttpResult>> GetCategory(
        Guid id,
        ICategoryService categoryService,
        IHttpContextAccessor httpContextAccessor
    )
    {
        try
        {
            var category = await categoryService.GetCategoryAsync(id);

            return TypedResults.Ok(category);
        }
        catch (NullReferenceException notFound)
        {
            ProblemDetails details = new ProblemDetails()
            {
                Title = "Category not found",
                Detail = notFound.Message,
                Status = StatusCodes.Status404NotFound,
                Instance = httpContextAccessor.HttpContext!.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            };
            return TypedResults.Problem(details);
            // return TypedResults.NotFound(id);
        }
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
            ProblemDetails details = new()
            {
                Title = "Category already exists",
                Detail = exists.Message,
                Status = StatusCodes.Status409Conflict,
            };
            return TypedResults.Problem(details);
        }
    }

    public record UpdateCategoryRequest(string Name, string? Description);

    private static async Task<
        Results<Ok<CategoryDTO>, NotFound<Guid>, BadRequest<UpdateCategoryRequest>>
    > UpdateCategory(
        [FromRoute] Guid categoryId,
        UpdateCategoryRequest request,
        ICategoryService categoryService
    )
    {
        if (categoryId == Guid.Empty)
            return TypedResults.BadRequest(request);

        if (string.IsNullOrWhiteSpace(request.Name))
            return TypedResults.BadRequest(request);

        if (Guid.TryParse(categoryId.ToString(), out var _))
            return TypedResults.BadRequest(request);

        try
        {
            var updatedCategory = await categoryService.UpdateCategoryAsync(
                categoryId,
                new CategoryDTO() { Name = request.Name, Description = request.Description }
            );
            return TypedResults.Ok(updatedCategory);
        }
        catch (NullReferenceException)
        {
            // ProblemDetails details = new ProblemDetails()
            // {
            //     Title = "Category not found",
            //     Detail = notFound.Message,
            //     Status = StatusCodes.Status404NotFound,
            // };
            // return TypedResults.Problem(details);

            return TypedResults.NotFound(categoryId);
        }
    }

    public record DeleteCategoryRequest(Guid Id);

    private static async Task<Results<Ok, ProblemHttpResult>> DeleteCategory(
        Guid id,
        ICategoryService categoryService,
        IHttpContextAccessor httpContextAccessor
    )
    {
        if (id == Guid.Empty)
        {
            ProblemDetails details = new ProblemDetails()
            {
                Title = "Invalid Id",
                Detail = "Id cannot be empty",
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContextAccessor.HttpContext!.Request.Path,
            };
            return TypedResults.Problem(details);
        }

        var isDeleted = await categoryService.DeleteCategoryAsync(id);
        if (isDeleted is false)
        {
            ProblemDetails details = new ProblemDetails()
            {
                Title = "Category not found",
                Detail = $"Category with id {id} not found",
                Status = StatusCodes.Status404NotFound,
                Instance = httpContextAccessor.HttpContext!.Request.Path,
            };
            return TypedResults.Problem(details);
        }

        return TypedResults.Ok();
    }
}
