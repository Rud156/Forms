using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Forms.Models.ResponseModels;
using Forms.Services;

namespace Forms.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FormsController : Controller
    {
        private readonly IFormService formService;

        public FormsController(IFormService formService)
        {
            this.formService = formService;
        }

        [HttpGet("{id}")]
        public async Task<object> GetForm(string id)
        {
            FormObjectViewModel form = await formService.GetForm(id);
            return Ok(new
            {
                success = true,
                form
            });
        }
    }
}