using System;
using Common.Enums;

namespace Domain.Models 
{
    public class Reminder
    {       
        public int Id { get; private set; }
        public bool Active { get; private set; }
        public DateTime Time { get; private set; }
        public ReminderTimeOffset TimeOffset { get; private set; }

        private Reminder() { }

        public static Reminder Create(
            bool active,
            DateTime time,
            ReminderTimeOffset timeOffset)
            {
                return new Reminder 
                {
                    Active = active,
                    Time = time,
                    TimeOffset = timeOffset
                };
            }
    }
}