using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Models;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApi.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [Produces("text/html")]
        public IActionResult Index() {
            // var path = "static-files/default-home-page.html";
            // var response = new HttpResponseMessage();
            // response.Content =  new StringContent(System.IO.File.ReadAllText(path));
            // response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            // return Ok(System.IO.File.ReadAllText(path));
            return Ok();
        }
    }
}