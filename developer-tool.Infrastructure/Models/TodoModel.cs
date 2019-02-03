using Infrastructure.Core;

namespace Infrastructure.Models 
{
    public class TodoModel : Entity
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsArchived { get; set; }

        // replace with user object later
        public int? UserId { get; set; }
    }
}