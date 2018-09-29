using System;
using Common.Enums;

namespace Domain.Models 
{
    public class Reminder
    {       
        public int Id { get; private set; }
        public DateTime Time { get; private set; }
        public ReminderTimeOffset TimeOffset { get; private set; }

        private Reminder() { }

        public static Reminder Create(
            DateTime time,
            ReminderTimeOffset timeOffset)
            {
                return new Reminder 
                {
                    Time = time,
                    TimeOffset = timeOffset
                };
            }

        public void Update(DateTime time, ReminderTimeOffset timeOffset) {
            Time = time;
            TimeOffset = timeOffset;
        }
    }
}