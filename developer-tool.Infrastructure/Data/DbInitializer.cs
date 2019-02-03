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
        private const bool _activeReminder = true;
        
        public static void Initialize(BackOfficeContext context)
        {
            context.Database.Migrate();
            context.Database.EnsureCreated();
        }

        public static void SeedEvents(BackOfficeContext context) {
            if (context.Set<EventModel>().Any())
            {
                return; // Db has been seeded;
            }

            var calendarEvents = new EventModel[]
            {
                CreateEvent("#00abff", "Test title event 1", "Test description event 1", new DateTime(2018, 9, 24), new DateTime(2018, 9, 26), CreateReminder(new DateTime(2018, 9, 24, 8, 40, 0), _activeReminder, ReminderTimeOffset.FifteenMinBefore)),
                CreateEvent("#00abff", "Test title event 2", "Test description event 2", new DateTime(2018, 9, 27)),
                CreateEvent("#00abff", "Test title event 3", "Test description event 3", new DateTime(2018, 9, 25), new DateTime(2018, 9, 30))
            };

            var todos = new TodoModel[]
            {
                CreateTodo("Test todo create", false, false),
                CreateTodo("Test todo completed", true, false),
                CreateTodo("Test todo uncompleted", false, false),
                CreateTodo("Test todo archived", true, true),
            };

            context.Set<EventModel>().AddRange(calendarEvents);
            context.Set<TodoModel>().AddRange(todos);
            context.SaveChanges();
        }

        public static EventModel CreateEvent(
            string color,
            string title,
            string description,
            DateTime start,
            DateTime? end = null,
            ReminderModel reminder = null)
        {
            return new EventModel {
                Color = color,
                Title = title,
                Description = description,
                Start = start,
                End = end,
                Reminder = reminder
            };
        }

        public static ReminderModel CreateReminder(DateTime time, bool active, ReminderTimeOffset timeOffset) 
        {
            return new ReminderModel {
                Time = time,
                Active = active,
                TimeOffset = timeOffset
            };
        }

        public static TodoModel CreateTodo(string description, bool isCompleted, bool isArchived)
        {
            return new TodoModel {
                Description = description,
                IsCompleted = isCompleted,
                IsArchived = isArchived
            };
        }
    }
}