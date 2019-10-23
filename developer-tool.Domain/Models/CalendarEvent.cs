using System;
using System.Collections.Generic;

namespace Domain.Models 
{
    public class CalendarEvent
    {
        public int Id { get; set; }
        public string Color { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime? End { get; private set; }
        public ICollection<Reminder> Reminders { get; private set; }

        private CalendarEvent() { }

        public static CalendarEvent Create(
            string color,
            string title,
            string description,
            DateTime start,
            DateTime? end = null)
        {
            return new CalendarEvent 
            {
                Color = color,
                Title = title,
                Description = description,
                Start = start,
                End = end,
                Reminders = new List<Reminder>()
            };
        }

        public CalendarEvent WithReminder(Reminder reminder) {
            this.Reminders.Add(reminder);
            return this;
        }
    }
}