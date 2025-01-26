namespace web.api.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public Guid categoryId { get; set; }
        public Category category { get; set; }
    }
}
