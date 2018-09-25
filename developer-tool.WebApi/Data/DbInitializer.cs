using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Infrastructure.Models;
using Common.Enums;

namespace WebApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BackOfficeContext context)
        {
            context.Database.Migrate();
            context.Database.EnsureCreated();
        }

        public static void Seed(BackOfficeContext context) {
            if (context.CalendarEvents.Any())
            {
                return; // Db has been seeded;
            }

            var items = new CalendarEvent[]
            {
                CalendarEvent.Create("#00abff", "Test title event 1", "Test description event 1", new DateTime(2018, 9, 24), new DateTime(2018, 9, 26), true, new DateTime(2018, 9, 24, 8, 40, 0), EventDateTimeOffset.FifteenMinBefore),
                CalendarEvent.Create("#00abff", "Test title event 2", "Test description event 2", new DateTime(2018, 9, 27)),
                CalendarEvent.Create("#00abff", "Test title event 3", "Test description event 3", new DateTime(2018, 9, 25), new DateTime(2018, 9, 30))
            };

            context.CalendarEvents.AddRange(items);
            context.SaveChanges();
        }
    }
}