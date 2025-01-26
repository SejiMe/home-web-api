namespace web.api.Models
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public required string Name { get; set; }
        public string? Description { get; set; } = string.Empty;

        public required TimeSpan timeStamp { get; set; }

        public ICollection<Todo> Todos { get; set; } = null!;
    }
}
