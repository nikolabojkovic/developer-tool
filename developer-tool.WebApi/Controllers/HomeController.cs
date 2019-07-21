using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [ApiExplorerSettings(IgnoreApi=true)]
        [Produces("text/html")]
        [HttpGet]
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