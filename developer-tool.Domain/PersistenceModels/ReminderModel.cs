using System;
using Domain.Enums;

namespace Domain.PersistenceModels
{
    public class ReminderModel : EntityModel
    {   
        public bool Active { get; set; }
        public DateTime Time { get; set; }
        public ReminderTimeOffset TimeOffset { get; set; }

        public int EventId { get; set; }
        public CalendarEventModel Event { get; set; }
    }
}