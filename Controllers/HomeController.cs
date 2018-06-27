using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Forms.Models;

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
