using System;
using Domain.Models;

namespace Domain.PersistenceModels
{
    public class CalendarEventModel : Entity
    {
        public string Color { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public ReminderModel Reminder { get; set; }
    }
}