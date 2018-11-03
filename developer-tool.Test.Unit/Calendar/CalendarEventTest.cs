using System;
using Xunit;
using Domain.Models;
using FluentAssertions;
using Common.Enums;

namespace TestUnit 
{
    public class CalendarEventTest
    {
        private const bool _activeReminder = true;

        [Theory]
        [InlineData("#00abff", "Test title event 1", "Test description event 1", "2018-9-25")]
        [InlineData("#00abff", "Test title event 1", "Test description event 1", "2018-9-25", "2018-9-30")]
        public void CalendarEventTest_CreateEvent_ShouldReturnEvent(
            string color, 
            string title, 
            string description,
            string start, 
            string end = null)
        {
            var optionalEndDate = end == null ? default(DateTime?) : DateTime.Parse(end);
            var startDate = DateTime.Parse(start);
            var expectedEvent = CalendarEvent.Create(color, title, description, startDate, optionalEndDate);

            expectedEvent.Color.Should().Be(color);
            expectedEvent.Title.Should().Be(title);
            expectedEvent.Description.Should().Be(description);
            expectedEvent.Start.Should().Be(startDate);
            expectedEvent.End.Should().Be(optionalEndDate);
        }

        [Theory]
        [InlineData("#00abff", "Test title event 1", "Test description event 1", "2018-9-25", "2018-9-30", _activeReminder, "2018/9/24 08:40:00", ReminderTimeOffset.FifteenMinBefore)]
        [InlineData("#00abff", "Test title event 1", "Test description event 1", "2018-9-25", "2018-9-30", !_activeReminder, "2018/9/24 08:40:00", ReminderTimeOffset.FifteenMinBefore)]
        public void CalendarEventTest_CreateEventWithReminder_ShouldReturnEvent(
            string color, 
            string title, 
            string description,
            string start,
            string end,
            bool reminderActive,
            string reminderTime,
            ReminderTimeOffset reminderTimeOffset)
        {
            var optionalEndDate = end == null ? default(DateTime?) : DateTime.Parse(end);
            var startDate = DateTime.Parse(start);
            var expectedEvent = CalendarEvent.Create(color, title, description, startDate, optionalEndDate)
                                             .WithReminder(Reminder.Create(
                                                                        reminderActive,
                                                                        DateTime.Parse(reminderTime),
                                                                        reminderTimeOffset));

            expectedEvent.Color.Should().Be(color);
            expectedEvent.Title.Should().Be(title);
            expectedEvent.Description.Should().Be(description);
            expectedEvent.Start.Should().Be(startDate);
            expectedEvent.End.Should().Be(optionalEndDate);
            expectedEvent.Reminder.Time.Should().Be(DateTime.Parse(reminderTime));
            expectedEvent.Reminder.Active.Should().Be(reminderActive);
            expectedEvent.Reminder.TimeOffset.Should().Be(reminderTimeOffset);
        }
    }
}