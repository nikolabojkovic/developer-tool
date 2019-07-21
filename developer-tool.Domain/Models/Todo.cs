using System;

namespace Domain.Models 
{
    [Serializable]
    public class Todo
    {
        public int Id { get; set; }
        public string Description { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsArchived { get; private set; }

        // replace with user object later
        public int? UserId { get; private set; }

        public Todo() {}

        public static Todo Create(string withDescription) 
        {
            return new Todo {
                Description = withDescription,
            };
        }

        public virtual void Complete() {
            IsCompleted = true;
        }

        public virtual void UnComplete() {
            IsCompleted = false;
        }

        public virtual void Archive() {
            IsArchived = true;
        }
    }
}