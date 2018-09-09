using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Interfaces;
using WebApi.ViewModels;

namespace WebApi.Controllers 
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var items = _testService.GetAllData();
            if (items == null)
            {
                return NotFound();
            }

            var result = items.ToList();
            return new ObjectResult(result);
        }

        [HttpGet("{id}", Name="GetById")]
        public IActionResult GetById(int id)
        {
            var item = _testService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TestViewModel item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            // replace this with auto mapper
            var newItem = new Test();
            newItem.Id = item.Id;
            newItem.FirstName = item.FirstName;

            _testService.Store(newItem);
            return CreatedAtRoute("GetById", new { id = newItem.Id }, newItem);
        }

        [HttpPut]
        public IActionResult Update([FromBody] TestViewModel item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var existingItem = _testService.GetById(item.Id);
            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Id = item.Id;
            existingItem.FirstName = item.FirstName;

            _testService.Update(existingItem);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _testService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }

            _testService.Remove(item);
            return new NoContentResult();
        }

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