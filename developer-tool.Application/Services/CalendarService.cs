using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Application.Interfaces;
using Domain.PersistenceModels;
using Domain.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IRepository<CalendarEventModel> _calendarEventRepository;
        private readonly IMapper _mapper;

        public CalendarService(
            IRepository<CalendarEventModel> calendarEventRepository,
            IMapper mapper)
        {
            _calendarEventRepository = calendarEventRepository;
            _mapper = mapper;
        }

        public IEnumerable<CalendarEvent> GetAllData()
        {
            var repoModels = _calendarEventRepository.FindAll()
                                                     .Include(x => x.Reminders);
            var domainModels = _mapper.Map<IEnumerable<CalendarEvent>>(repoModels);
            return domainModels;
        }

        public CalendarEvent GetById(int id)
        {
            var repoModel = _calendarEventRepository.Find(x => x.Id == id)
                                                    .Include(x => x.Reminders)
                                                    .FirstOrDefault();
            var domainModel = _mapper.Map<CalendarEvent>(repoModel);
            return domainModel;
        }

        public void Store(CalendarEvent item)
        {
            var repoModel = _mapper.Map<CalendarEventModel>(item);
            _calendarEventRepository.Add(repoModel);
        }

        public void Update(CalendarEvent entity)
        {
            var existingItem = _calendarEventRepository.Find(x => x.Id == entity.Id)
                                                       .Include(x => x.Reminders)
                                                       .FirstOrDefault();            
            existingItem = _mapper.Map<CalendarEvent, CalendarEventModel>(entity, existingItem);
            _calendarEventRepository.Update(existingItem);
        }

        public void Remove(int id)
        {
            var existingItem = _calendarEventRepository.Find(x => x.Id == id)
                                                       .FirstOrDefault();
            _calendarEventRepository.Delete(existingItem);
        }
    }
}