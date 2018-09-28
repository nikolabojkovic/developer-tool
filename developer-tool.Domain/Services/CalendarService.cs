using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Domain.Interfaces;
using Infrastructure.Models;
using Domain.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IRepository<Event> _calendarEventRepository;
        private readonly IMapper _mapper;

        public CalendarService(
            IRepository<Event> calendarEventRepository,
            IMapper mapper)
        {
            _calendarEventRepository = calendarEventRepository;
            _mapper = mapper;
        }

        public IEnumerable<CalendarEvent> GetAllData()
        {
            var repoModels = _calendarEventRepository.FindAll()
                                                     .Include(x => x.Reminder);
            var domainModels = _mapper.Map<IEnumerable<CalendarEvent>>(repoModels);
            return domainModels;
        }

        public CalendarEvent GetById(int id)
        {
            var repoModel = _calendarEventRepository.Find(x => x.Id == id)
                                                    .Include(x => x.Reminder)
                                                    .FirstOrDefault();
            var domainModel = _mapper.Map<CalendarEvent>(repoModel);
            return domainModel;
        }

        public void Store(CalendarEvent item)
        {
            var repoModel = _mapper.Map<Event>(item);
            _calendarEventRepository.Add(repoModel);
        }

        public void Update(CalendarEvent entity)
        {
            var existingItem = _calendarEventRepository.Find(x => x.Id == entity.Id)
                                                       .FirstOrDefault();            
            _mapper.Map<CalendarEvent, Event>(entity, existingItem);
            _calendarEventRepository.Update(existingItem);
        }

        public void Remove(CalendarEvent entity)
        {
            _calendarEventRepository.Delete(_mapper.Map<Event>(entity));
        }
    }
}