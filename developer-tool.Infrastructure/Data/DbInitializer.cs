using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DbContexts;
using Infrastructure.Models;
using Common.Enums;

namespace Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BackOfficeContext context)
        {
            context.Database.Migrate();
            context.Database.EnsureCreated();
        }

        public static void SeedEvents(BackOfficeContext context) {
            if (context.CalendarEvents.Any())
            {
                return; // Db has been seeded;
            }

            var items = new Event[]
            {
                CreateEvent("#00abff", "Test title event 1", "Test description event 1", new DateTime(2018, 9, 24), new DateTime(2018, 9, 26), CreateReminder(new DateTime(2018, 9, 24, 8, 40, 0), ReminderTimeOffset.FifteenMinBefore)),
                CreateEvent("#00abff", "Test title event 2", "Test description event 2", new DateTime(2018, 9, 27)),
                CreateEvent("#00abff", "Test title event 3", "Test description event 3", new DateTime(2018, 9, 25), new DateTime(2018, 9, 30))
            };

            context.CalendarEvents.AddRange(items);
            context.SaveChanges();
        }

        public static Event CreateEvent(
            string color,
            string title,
            string description,
            DateTime start,
            DateTime? end = null,
            Reminder reminder = null)
        {
            return new Event {
                Color = color,
                Title = title,
                Description = description,
                Start = start,
                End = end,
                Reminder = reminder
            };
        }

        public static Reminder CreateReminder(DateTime time, ReminderTimeOffset timeOffset) 
        {
            return new Reminder {
                Time = time,
                TimeOffset = timeOffset
            };
        }
    }
}