namespace web.api.Models
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public required string Name { get; set; }
        public string? Description { get; set; } = string.Empty;

        public bool IsActive { get; private set; } = true;
        public required DateTime TimeStamp { get; set; }

        public ICollection<Todo> Todos { get; set; } = null!;

        public void ActivateCategory()
        {
            IsActive = true;
        }

        public void DeactiveCategory()
        {
            IsActive = false;
        }
    }
}
