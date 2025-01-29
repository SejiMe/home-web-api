using web.api.Infrastructure.Data;
using web.api.Interfaces;
using web.api.Models;

namespace web.api.Services;

public class TodoService : ITodoService
{
    private readonly HomeDbContext _context;

    public TodoService(HomeDbContext context)
    {
        _context = context;
    }

    public Task Activate(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Todo> CreateTodoAsync(Todo todo)
    {
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();
        return todo;
    }

    public Task Deactivate(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteTodoAsync(Guid id)
    {
        var todo = _context.Todos.Find(id);

        if (todo is null)
            return false;

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Todo> GetTodoAsync(Guid id)
    {
        var todoRes = await _context.Todos.FindAsync(id);
        return todoRes ?? throw new NullReferenceException();
    }

    public Task<IEnumerable<Todo>> GetTodosAsync()
    {
        return Task.FromResult(_context.Todos.AsEnumerable());
    }

    public Task<Todo> UpdateTodoAsync(Guid id, Todo todo)
    {
        var todoEntity = _context.Todos.Find(id) ?? throw new NullReferenceException();

        if (todoEntity.Name != todo.Name)
            todoEntity.Name = todo.Name;

        if (todo.IsComplete)
            todoEntity.CompleteTask();
        else
            todoEntity.IncompleteTask();

        todoEntity.categoryId = todo.categoryId;
        todoEntity.category = todo.category;
        todoEntity.Name = todo.Name;

        _context.Todos.Update(todoEntity);

        _context.SaveChanges();
        return Task.FromResult(todoEntity);
    }
}
