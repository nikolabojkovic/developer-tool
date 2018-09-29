using System;
using Common.Enums;
using Infrastructure.Core;

namespace Infrastructure.Models 
{
    public class Reminder : Entity
    {       
        public DateTime Time { get; set; }
        public ReminderTimeOffset TimeOffset { get; set; }
        public Event Event { get; set; }
    }
}