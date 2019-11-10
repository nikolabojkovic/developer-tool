using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApi.ViewModels;
using AutoMapper;
using Application.Interfaces;
using WebApi.Results;
using WebApi.InputModels;
using Domain.Models;
using WebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

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
            var viewModels = _mapper.Map<IEnumerable<TodoViewModel>>(items);
            return SuccessObjectResult.Data(viewModels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _todoService.GetByIdAsync(id);
            if (item == null) return NotFound();

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

        [HttpGet("test")]
        [Authorize]
        public IActionResult Test() 
        {
            return Ok("It's ok.");
        }

        [HttpPut("{id}")]
        [TransactionFilter]
        public async Task<IActionResult> Update(int id, [FromBody] TodoInputModel item)
        {
            // Guards
            if (item == null) return BadRequest();
            var existingItem = await _todoService.GetByIdAsync(id);
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
        public async Task<IActionResult> Delete(int id)
        {
             var item = await _todoService.GetByIdAsync(id);
            if (item == null) return NotFound();

            _todoService.Remove(id);
            return NoContent();
        }

        [HttpPut("complete/{id}")]
        [TransactionFilter]
        public async Task<IActionResult> Complete(int id)
        {
            // Guards
            var existingItem = await _todoService.GetByIdAsync(id);
            if (existingItem == null) return NotFound();

            // Update logic
            _todoService.Complete(id);

            // Return update ok result
            return NoContent();
        }
        
        [HttpPut("uncomplete/{id}")]
        [TransactionFilter]
        public async Task<IActionResult> UnComplete(int id)
        {
            // Guards
            var existingItem = await _todoService.GetByIdAsync(id);
            if (existingItem == null) return NotFound();

            // Update logic
            _todoService.UnComplete(id);

            // Return update ok result
            return NoContent();
        }

        [HttpPut("archive/{id}")]
        [TransactionFilter]
        public async Task<IActionResult> Archive(int id)
        {
            // Guards
            var existingItem = await _todoService.GetByIdAsync(id);
            if (existingItem == null) return NotFound();

            // Update logic
            _todoService.Archive(id);

            // Return update ok result
            return NoContent();
        }
    }
}