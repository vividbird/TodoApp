namespace TodoApp
{
    public class Task
    {
        public string Name { get; private set; }
        public bool IsCompleted { get; private set; }

        public Task(string name, bool isCompleted)
        {
            this.Name = name;
            this.IsCompleted = isCompleted;
        }
    }
}
