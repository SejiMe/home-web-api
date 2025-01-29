namespace web.api.Models
{
    public class Todo
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public string Name { get; set; }
        public bool IsComplete { get; private set; }
        public Guid categoryId { get; set; }
        public Category category { get; set; }

        public void CompleteTask()
        {
            IsComplete = true;
        }

        public void IncompleteTask()
        {
            IsComplete = false;
        }
    }
}
