namespace web.api.Models
{
    ///
    public class Todo
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public string Name { get; set; }
        public bool IsComplete { get; private set; }
        public Guid categoryId { get; set; }
        public Category category { get; set; }

        public void CompleteTask()
        {
            if (IsComplete is true)
            {
                throw new InvalidOperationException("Task is already completed");
            }

            IsComplete = true;
        }

        public void IncompleteTask()
        {
            if (IsComplete is not true)
                throw new InvalidOperationException("Task is already incomplete");

            IsComplete = false;
        }
    }
}
