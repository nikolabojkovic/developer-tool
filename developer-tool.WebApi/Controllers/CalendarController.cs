using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Models;
using Domain.Interfaces;
using WebApi.ViewModels;
using WebApi.Filters;
using AutoMapper;
using System.Collections.Generic;

namespace WebApi.Controllers 
{
    [Route("api/calendar/events")]
    public class CalendarEventsController : Controller
    {
        private readonly ICalendarService _calendarService;
        private readonly IMapper _mapper;

        public CalendarEventsController(ICalendarService testService, IMapper mapper)
        {
            _calendarService = testService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var items = _calendarService.GetAllData();
            if (items == null)
            {
                return NotFound();
            }

            return new ObjectResult(_mapper.Map<IEnumerable<CalendarEventViewModel>>(items));
        }

        // [HttpGet("{id}", Name="GetById")]
        // public IActionResult GetById(int id)
        // {
        //     var item = _testService.GetById(id);
        //     if (item == null)
        //     {
        //         return NotFound();
        //     }

        //     return new ObjectResult(item);
        // }

        // [HttpPost]
        // [TransactionFilter]
        // public IActionResult Create([FromBody] TestViewModel item)
        // {
        //     if (item == null)
        //     {
        //         return BadRequest();
        //     }

        //     // replace this with auto mapper
        //     var newItem = new Test();
        //     newItem.Id = item.Id;
        //     newItem.FirstName = item.FirstName;

        //     _testService.Store(newItem);
        //     return CreatedAtRoute("GetById", new { id = newItem.Id }, newItem);
        // }

        // [HttpPut]
        // [TransactionFilter]
        // public IActionResult Update([FromBody] TestViewModel item)
        // {
        //     if (item == null)
        //     {
        //         return BadRequest();
        //     }

        //     var existingItem = _testService.GetById(item.Id);
        //     if (existingItem == null)
        //     {
        //         return NotFound();
        //     }

        //     existingItem.Id = item.Id;
        //     existingItem.FirstName = item.FirstName;

        //     _testService.Update(existingItem);
        //     return new NoContentResult();
        // }

        // [HttpDelete("{id}")]
        // [TransactionFilter]
        // public IActionResult Delete(int id)
        // {
        //     var item = _testService.GetById(id);
        //     if (item == null)
        //     {
        //         return NotFound();
        //     }

        //     _testService.Remove(item);
        //     return new NoContentResult();
        // }

        // move to separate controller
        [HttpGet("~/api/download")]
        public Cv cv() 
        {
            var cv = new Cv {
                Name = "Nikola Bojkovic CV",
                Type = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                Document = System.IO.File.ReadAllBytes("static-files/Nikola Bojkovic CV.docx")
            };

            return cv;
        }
    }
}