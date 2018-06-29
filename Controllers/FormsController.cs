using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Forms.Services;
using Forms.ServiceInterfaces;
using Forms.Models.DBModels;
using Forms.Models.NewModels;
using Forms.Models.ResponseModels;
using Forms.Models.RequestModels;

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

        [HttpGet("{formId}")]
        public async Task<object> GetForm(string formId)
        {
            bool formIdParseSuccess = ObjectId.TryParse(formId, out ObjectId formObjectId);
            if (!formIdParseSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Form Id"
                });

            FormObjectViewModel form = await formService.GetForm(formObjectId);
            if (form == null)
                return NotFound(new
                {
                    success = false,
                    message = "Incorrect Form Requested"
                });

            return Ok(new
            {
                success = true,
                form
            });
        }

        [HttpGet("{formId}/field/{fieldId}")]
        public async Task<object> GetField(string formId, string fieldId)
        {
            bool formIdParseSuccess = ObjectId.TryParse(formId, out ObjectId formObjectId);
            bool fieldIdParseSuccess = ObjectId.TryParse(fieldId, out ObjectId fieldObjectId);
            if (!formIdParseSuccess || !fieldIdParseSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Id(s) Provided"
                });

            FieldViewModel field = await formService.GetField(formObjectId, fieldObjectId);
            if (field == null)
                return NotFound(new
                {
                    success = false,
                    message = "Incorrect Field Requested"
                });

            return Ok(new
            {
                success = true,
                field
            });
        }

        [HttpGet("user/{username}")]
        public async Task<object> GetFormUser(string username)
        {
            IEnumerable<FormViewModel> forms = await formService.GetFormsCreatedBy(username);
            return Ok(new
            {
                success = true,
                forms
            });
        }

        [HttpPost]
        public async Task<object> CreateForm([FromBody] NewFormViewModel form)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Request Body"
                });

            try
            {
                FormObjectViewModel savedForm = await formService.CreateForm(form);
                return Created($"{Request.Host}{Request.PathBase}{Request.Path}",
                new
                {
                    success = true,
                    form = savedForm
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new JsonResult(new
                {
                    success = false,
                    message = e.Message
                })
                {
                    StatusCode = 500
                };
            }

        }

        [HttpPut("{formId}/field")]
        public async Task<object> AddNewFieldToForm(string formId, [FromBody] NewFieldViewModel field)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Request Body"
                });

            bool parseSuccess = ObjectId.TryParse(formId, out ObjectId formObjectId);
            if (!parseSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Form Id"
                });

            try
            {
                FieldViewModel savedField = await formService.AddNewFieldToForm(field, formObjectId);
                return Created($"{Request.Host}{Request.PathBase}{Request.Path}",
                new
                {
                    success = true,
                    field = savedField
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new JsonResult(new
                {
                    success = false,
                    message = e.Message
                })
                {
                    StatusCode = 500
                };
            }

        }

        [HttpPatch("{formId}")]
        public async Task<object> UpdateFormTitle(string formId, [FromBody] FormTitle formTitleObject)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Request Body"
                });

            bool parseSuccess = ObjectId.TryParse(formId, out ObjectId formObjectId);
            if (!parseSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Form Id"
                });

            try
            {
                await formService.UpdateFormTitle(formObjectId, formTitleObject.title);
                return Ok(new
                {
                    success = true,
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new JsonResult(new
                {
                    success = false,
                    message = e.Message
                })
                {
                    StatusCode = 500
                };
            }
        }

        [HttpPatch("{formId}/field/{fieldId}")]
        public async Task<object> UpdateField(string formId, string fieldId, [FromBody]FieldViewModel field)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Request Body"
                });

            bool formIdParseSuccess = ObjectId.TryParse(formId, out ObjectId formObjectId);
            bool fieldIdParseSuccess = ObjectId.TryParse(fieldId, out ObjectId fieldObjectId);
            if (!formIdParseSuccess || !fieldIdParseSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Id(s) Provided"
                });

            try
            {
                FieldViewModel updatedField = await formService.UpdateField(field, formObjectId, fieldObjectId);
                return Ok(new
                {
                    success = true,
                    field = updatedField
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new JsonResult(new
                {
                    success = false,
                    messsage = e.Message
                })
                {
                    StatusCode = 500
                };
            }
        }

        [HttpDelete("{formId}/field/{fieldId}")]
        public async Task<object> DeleteField(string formId, string fieldId)
        {
            bool formIdParseSuccess = ObjectId.TryParse(formId, out ObjectId formObjectId);
            bool fieldIdParseSuccess = ObjectId.TryParse(fieldId, out ObjectId fieldObjectId);
            if (!formIdParseSuccess || !fieldIdParseSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Id(s) Provided"
                });

            try
            {
                await formService.DeleteField(formObjectId, fieldObjectId);
                return Ok(new
                {
                    success = true,
                    message = "Field Deleted Successfully"
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new JsonResult(new
                {
                    success = false,
                    messsage = e.Message
                })
                {
                    StatusCode = 500
                };
            }
        }

        [HttpDelete("{formId}")]
        public async Task<object> DeleteForm(string formId)
        {
            bool formIdParseSuccess = ObjectId.TryParse(formId, out ObjectId formObjectId);
            if (!formIdParseSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid formId Id"
                });

            try
            {
                await formService.DeleteForm(formObjectId);
                return Ok(new
                {
                    success = true,
                    message = "Form Deleted Successfully"
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new JsonResult(new
                {
                    success = false,
                    messsage = e.Message
                })
                {
                    StatusCode = 500
                };
            }
        }

        [HttpDelete("user/{username}")]
        public async Task<object> DeleteFormsCreatedBy(string username)
        {
            try
            {
                long totalDeleted = await formService.DeleteFormsCreatedBy(username);
                return Ok(new
                {
                    success = false,
                    message = "Forms Deleted",
                    totalDeleted
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new JsonResult(new
                {
                    success = false,
                    messsage = e.Message
                })
                {
                    StatusCode = 500
                };
            }
        }
    }
}