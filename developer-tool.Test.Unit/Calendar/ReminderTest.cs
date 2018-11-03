using System;
using Xunit;
using Domain.Models;
using FluentAssertions;
using Common.Enums;

namespace TestUnit 
{
    public class ReminderTest
    {
        private const bool _activeReminder = true;

        [Theory]
        [InlineData(_activeReminder, "2018/9/24 08:40:00", ReminderTimeOffset.FifteenMinBefore)]
        [InlineData(!_activeReminder, "2018/9/24 08:40:00", ReminderTimeOffset.FifteenMinBefore)]
        public void CalendarEventReminderTest_CreateReminder_ShouldReturnReminder(
            bool active,
            string time,
            ReminderTimeOffset timeOffset)
        {
            var expectedResult = Reminder.Create(active, DateTime.Parse(time), timeOffset);

            expectedResult.Time.Should().Be(DateTime.Parse(time));
            expectedResult.TimeOffset.Should().Be(timeOffset);
        }
    }
}