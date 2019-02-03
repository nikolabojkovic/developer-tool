

namespace WebApi.ViewModels
{
    public class TodoViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsArchived { get; set; }
    }
}