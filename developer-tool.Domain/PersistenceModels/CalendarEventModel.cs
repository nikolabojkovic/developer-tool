using System;
using System.Collections.Generic;

namespace Domain.PersistenceModels
{
    public class CalendarEventModel : EntityModel
    {
        public string Color { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public ICollection<ReminderModel> Reminders { get; set; }
    }
}