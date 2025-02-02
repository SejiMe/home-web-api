using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using web.api.Infrastructure.Data;
using web.api.Interfaces;
using web.api.Models;
using web.api.Services;

namespace web.api.Endpoints
{
    public static class TodoEndpoint
    {
        public static void RegisterTodoEndpoint(this IEndpointRouteBuilder builder)
        {
            builder
                .MapGroup("todos")
                .MapGet("", GetAllTodo)
                .WithTags("Todos Endpoint")
                .WithDescription("Get all todo items")
                .Produces<IReadOnlyList<Todo>>(StatusCodes.Status200OK);

            builder
                .MapGroup("todos")
                .MapGet("{id:guid}", GetTodoItem)
                .WithTags("Todos Endpoint")
                .WithDescription("Get a todo item by id")
                .Produces<Todo>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            builder.MapGroup("todos").MapPost("", PostTodoItem).WithTags("Todos Endpoint");
            builder.MapGroup("todos").MapPut("{id:guid}", PutTodoItem).WithTags("Todos Endpoint");
            builder
                .MapGroup("todos")
                .MapDelete("{id:guid}", DeleteTodoItem)
                .WithTags("Todos Endpoint");
            // builder.MapGet("/todos/{id:guid}/complete", CompleteTodoItem);
        }

        public static async Task<IResult> GetAllTodo(ITodoService todoService)
        {
            var todoList = await todoService.GetTodosAsync();

            if (todoList == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(todoList);
        }

        public static async Task<IResult> GetTodoItem(Guid id, ITodoService todoService)
        {
            var todoItem = await todoService.GetTodoAsync(id);

            if (todoItem == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(todoItem);
        }

        public static async Task<Results<Created<Todo>, BadRequest>> PostTodoItem(
            Todo todo,
            ITodoService todoService
        )
        {
            var createdTodo = await todoService.CreateTodoAsync(todo);
            return TypedResults.Created("CreateTodoItem", createdTodo);
        }

        public static async Task<Results<NoContent, BadRequest, NotFound<Guid>>> PutTodoItem(
            Guid id,
            Todo todo,
            ITodoService todoService
        )
        {
            if (todo.Id != id)
                return TypedResults.BadRequest();

            try
            {
                await todoService.UpdateTodoAsync(id, todo);
                return TypedResults.NoContent();
            }
            catch (NullReferenceException)
            {
                return TypedResults.NotFound(id);
            }
        }

        public static async Task<Results<NoContent, NotFound, BadRequest>> DeleteTodoItem(
            Guid id,
            ITodoService todoService
        )
        {
            var todoItem = await todoService.GetTodoAsync(id);

            if (todoItem == null)
            {
                return TypedResults.NotFound();
            }

            await todoService.DeleteTodoAsync(id);

            return TypedResults.NoContent();
        }

        private static bool TodoItemExists(Guid id, ITodoService todoService)
        {
            var res = todoService.GetTodoAsync(id).Result;
            return res != null;
        }
    }
}
