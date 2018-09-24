using System;
using Common.Enums;

namespace WebApi.ViewModels {
    public class CalendarEventViewModel {
        public int Id { get; set; }
        public string Color { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public bool IsReminderEnabled { get; set; }
        public DateTime? ReminderTime { get; set; }
        public EventDateTimeOffset? ReminderTimeOffset { get; set; }
    }
}