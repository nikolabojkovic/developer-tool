namespace Domain.PersistenceModels 
{
    public class TodoModel : EntityModel
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsArchived { get; set; }

        // replace with user object later
        public int? UserId { get; set; }
    }
}