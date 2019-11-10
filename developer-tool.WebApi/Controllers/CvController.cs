using AutoMapper;
using Domain.PersistenceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers 
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class CvController : Controller
    {
        private readonly IMapper _mapper;

        public CvController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("download")]
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