using System;
using Common.Enums;
using Infrastructure.Core;

namespace Infrastructure.Models 
{
    public class EventModel : Entity
    {
        public string Color { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public ReminderModel Reminder { get; set; }
    }
}