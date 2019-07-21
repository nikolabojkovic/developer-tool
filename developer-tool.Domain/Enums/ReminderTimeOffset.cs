using System;

namespace Domain.Enums
{
    public enum ReminderTimeOffset
    {
        Undefined = 0,
        AtTimeOfEvent = 1,
        FiveMinBefore = 2,
        TenMinBefore = 3,
        FifteenMinBefore = 4,
        ThirtyMinBefore = 5,
        OneHoureBefore = 6
    }
}
