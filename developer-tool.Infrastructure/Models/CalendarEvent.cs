using System;
using Common.Enums;

namespace Infrastructure.Models 
{
    public class CalendarEvent : Entity
    {
        public string Color { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime? End { get; private set; }
        public bool IsReminderEnabled { get; private set; }
        public DateTime? ReminderTime { get; private set; }
        public EventDateTimeOffset? ReminderTimeOffset { get; private set; }

        private CalendarEvent() { }

        public static CalendarEvent Create(
            string color,
            string title,
            string description,
            DateTime start,
            DateTime? end = null,
            bool isReminderEnabled = false,
            DateTime? reminderTime = null,
            EventDateTimeOffset? reminderTimeOffset = null)
            {
                return new CalendarEvent 
                {
                    Color = color,
                    Title = title,
                    Description = description,
                    Start = start,
                    End = end,
                    IsReminderEnabled = isReminderEnabled,
                    ReminderTime = reminderTime,
                    ReminderTimeOffset = reminderTimeOffset
                };
            }

        public void Update(string title) {
            Title = title;
        }
    }
}