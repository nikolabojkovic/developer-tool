namespace Infrastructure.Models 
{
    public class TodoItem : Entity
    {
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}