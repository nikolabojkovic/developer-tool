using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Models;
using Domain.Interfaces;
using WebApi.ViewModels;
using WebApi.Filters;
using AutoMapper;
using System.Collections.Generic;
using WebApi.InputModels;

namespace WebApi.Controllers 
{
    [Route("api/[controller]")]
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