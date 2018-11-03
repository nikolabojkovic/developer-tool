using System;
using Common.Enums;

namespace WebApi.InputModels 
{
    public class CalendarEventInputModel {
        public string Color { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public ReminderInputModel Reminder { get; set; }
    }
}