using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Forms.Models.ResponseModels;
using Forms.Services;
using Forms.ServiceInterfaces;

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
            bool parseSuccess = ObjectId.TryParse(id, out ObjectId formId);
            if (!parseSuccess)
                return BadRequest(new
                {
                    parseSuccess = false,
                    message = "Invalid Object Id"
                });

            FormObjectViewModel form = await formService.GetForm(formId);
            if (form == null)
                return UnprocessableEntity(new
                {
                    success = false,
                    message = "Invalid Form Id Supplied"
                });

            return Ok(new
            {
                success = true,
                form
            });
        }
    }
}