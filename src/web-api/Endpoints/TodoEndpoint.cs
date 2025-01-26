using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using web.api.Infrastructure.Data;
using web.api.Models;

namespace web.api.Endpoints
{
    public static class TodoEndpoint
    {
        private static HomeDbContext _context;

        public static void RegisterTodoEndpoint(
            this IEndpointRouteBuilder builder,
            HomeDbContext context
        )
        {
            _context = context;
        }

        public static async Task<ActionResult<Todo>> GetTodoItem(int id)
        {
            var todoItem = await _context.Todos.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        public static async Task<ActionResult<Todo>> PostTodoItem(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = todo.Id }, todo);
        }

        public static async Task<IActionResult> PutTodoItem(int id, Todo todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }

            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        public static async Task<IActionResult> DeleteTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static bool TodoItemExists(int id) => _context.TodoItems.Any(e => e.Id == id);
    }
}
