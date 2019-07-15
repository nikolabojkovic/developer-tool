using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Application.Interfaces;
using WebApi.ViewModels;
using WebApi.Filters;
using AutoMapper;
using System.Collections.Generic;
using WebApi.InputModels;
using WebApi.Results;

namespace WebApi.Controllers 
{
    [Route("api/calendar/events")]
    public class CalendarEventsController : Controller
    {
        private readonly ICalendarService _calendarService;
        private readonly IMapper _mapper;

        public CalendarEventsController(ICalendarService calendarService, IMapper mapper)
        {
            _calendarService = calendarService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var items = _calendarService.GetAllData();
            if (items == null) return NoContent();
            var viewModels = _mapper.Map<IEnumerable<CalendarEventViewModel>>(items);
            return SuccessObjectResult.Data(viewModels);
        }

        [HttpGet("{id}")]
        public IActionResult GetBy(int id)
        {
            var item = _calendarService.GetById(id);
            if (item == null) return NoContent();

            return SuccessObjectResult.Data(_mapper.Map<CalendarEventViewModel>(item));
        }

        [HttpPost]
        [TransactionFilter]
        public IActionResult Post([FromBody] CalendarEventInputModel item)
        {
            if (item == null) return BadRequest();

            // replace with command pattern or builder pattern 
            var newItem = CalendarEvent.Create(
                item.Color,
                item.Title,
                item.Description,
                item.Start,
                item.End);

            if (item.Reminder != null)    
                newItem.WithReminder(
                    Reminder.Create(
                        item.Reminder.Active,
                        item.Reminder.Time,
                        item.Reminder.TimeOffset));

            _calendarService.Store(newItem);
            return Ok();
        }

        [HttpPut("{id}")]
        [TransactionFilter]
        public IActionResult Put(int id, [FromBody] CalendarEventInputModel item)
        {
            // Guards
            if (item == null) return BadRequest();
            var existingItem = _calendarService.GetById(id);
            if (existingItem == null) return NotFound();

            var mappedItem = _mapper.Map<CalendarEvent>(item);
            mappedItem.Id = id;

            // Update logic
            _calendarService.Update(mappedItem);

            // Return update ok result
            return NoContent();
        }

        [HttpDelete("{id}")]
        [TransactionFilter]
        public IActionResult Delete(int id)
        {
            var item = _calendarService.GetById(id);
            if (item == null) return NotFound();

            _calendarService.Remove(id);
            return NoContent();
        }
    }
}