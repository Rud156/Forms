using Microsoft.AspNetCore.Mvc;

namespace Forms.Controllers
{
    [Produces("application/json")]
    [Route(""), Route("api")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new
            {
                success = true,
                message = "Hello World"
            });
        }
    }
}
