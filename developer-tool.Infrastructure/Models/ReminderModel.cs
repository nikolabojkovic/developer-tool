using System;
using Common.Enums;
using Infrastructure.Core;

namespace Infrastructure.Models 
{
    public class ReminderModel : Entity
    {   
        public bool Active { get; set; }
        public DateTime Time { get; set; }
        public ReminderTimeOffset TimeOffset { get; set; }
        public CalendarEventModel Event { get; set; }
    }
}