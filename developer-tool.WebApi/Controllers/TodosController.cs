using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.DbContexts;
using Infrastructure.Models;
using System.Linq;
using WebApi.ViewModels;
using AutoMapper;
using Domain.Interfaces;
using WebApi.Results;
using WebApi.InputModels;
using Domain.Models;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TodosController : Controller
    {
        private readonly ITodoService _todoService;
        private readonly IMapper _mapper;
        public TodosController(
            ITodoService todoService, IMapper mapper
            )
        {
            _todoService = todoService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var items = _todoService.GetAllData();
            if (items == null) return NoContent();
            var viewModels = _mapper.Map<IEnumerable<TodoViewModel>>(items);
            return SuccessObjectResult.Data(viewModels);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _todoService.GetById(id);
            if (item == null) return NoContent();

            return SuccessObjectResult.Data(_mapper.Map<TodoViewModel>(item));
        }

        [HttpPost]
        [TransactionFilter]
        public IActionResult Create([FromBody] TodoInputModel item)
        {
            if (item == null) return BadRequest();

            // replace with command pattern or builder pattern or mediatR
            var newItem = Todo.Create(
                item.Description);

            _todoService.Store(newItem);
            return Ok();
        }

        [HttpPut("{id}")]
        [TransactionFilter]
        public IActionResult Update(int id, [FromBody] TodoInputModel item)
        {
            // Guards
            if (item == null) return BadRequest();
            var existingItem = _todoService.GetById(id);
            if (existingItem == null) return NotFound();

            var mappedItem = _mapper.Map<Todo>(item);
            mappedItem.Id = id;

            // Update logic
            _todoService.Update(mappedItem);

            // Return update ok result
            return NoContent();
        }

        [HttpDelete("{id}")]
        [TransactionFilter]
        public IActionResult Delete(int id)
        {
             var item = _todoService.GetById(id);
            if (item == null) return NotFound();

            _todoService.Remove(id);
            return NoContent();
        }

        [HttpPut("complete/{id}")]
        [TransactionFilter]
        public IActionResult Complete(int id)
        {
            // Guards
            var existingItem = _todoService.GetById(id);
            if (existingItem == null) return NotFound();

            // Update logic
            _todoService.Complete(id);

            // Return update ok result
            return NoContent();
        }
        
        [HttpPut("uncomplete/{id}")]
        [TransactionFilter]
        public IActionResult UnComplete(int id)
        {
            // Guards
            var existingItem = _todoService.GetById(id);
            if (existingItem == null) return NotFound();

            // Update logic
            _todoService.UnComplete(id);

            // Return update ok result
            return NoContent();
        }

        [HttpPut("archive/{id}")]
        [TransactionFilter]
        public IActionResult Archive(int id)
        {
            // Guards
            var existingItem = _todoService.GetById(id);
            if (existingItem == null) return NotFound();

            // Update logic
            _todoService.Archive(id);

            // Return update ok result
            return NoContent();
        }
    }
}