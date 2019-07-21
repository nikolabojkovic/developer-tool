using System.Collections.Generic;
using Domain.Models;

namespace Application.Interfaces
{
    public interface ICalendarService
    {
        IEnumerable<CalendarEvent> GetAllData();
        CalendarEvent GetById(int id);
        void Store(CalendarEvent item);
        void Update(CalendarEvent item);
        void Remove(int id);
    }
}