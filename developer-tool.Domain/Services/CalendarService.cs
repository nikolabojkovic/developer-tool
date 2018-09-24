using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Domain.Interfaces;
using Infrastructure.Models;

namespace Domain.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IRepository<CalendarEvent> _calendarEventRepository;
        
        public CalendarService(IRepository<CalendarEvent> calendarEventRepository)
        {
            _calendarEventRepository = calendarEventRepository;
        }

        public IEnumerable<CalendarEvent> GetAllData()
        {
            return _calendarEventRepository.FindAll();
        }

        public CalendarEvent GetById(int id)
        {
            return _calendarEventRepository.Find(x => x.Id == id).FirstOrDefault();
        }

        public void Store(CalendarEvent entity)
        {
            _calendarEventRepository.Add(entity);
        }

        public void Update(CalendarEvent entity)
        {
            var existingItem = GetById(entity.Id);
            existingItem.Update(entity.Title);
            
            _calendarEventRepository.Update(existingItem);
        }

        public void Remove(CalendarEvent entity)
        {
            _calendarEventRepository.Delete(entity);
        }
    }
}