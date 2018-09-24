using System.Collections.Generic;
using Infrastructure.Models;

namespace Domain.Interfaces
{
    public interface ICalendarService
    {
        IEnumerable<CalendarEvent> GetAllData();
        CalendarEvent GetById(int id);
        void Store(CalendarEvent entity);
        void Update(CalendarEvent entity);
        void Remove(CalendarEvent entity);
    }
}