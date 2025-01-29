using web.api.Models;

namespace web.api.Interfaces
{
    public interface ITodoService
    {
        Task<IEnumerable<Todo>> GetTodosAsync();
        Task<Todo> GetTodoAsync(Guid id);
        Task<Todo> CreateTodoAsync(Todo todo);
        Task<Todo> UpdateTodoAsync(Guid id, Todo todo);

        Task<bool> DeleteTodoAsync(Guid id);

        Task Activate(Guid id);

        Task Deactivate(Guid id);
    }
}
