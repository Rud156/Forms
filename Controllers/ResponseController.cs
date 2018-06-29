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
    public class ResponseController : Controller
    {
        private readonly IResponseService responseService;

        public ResponseController(IResponseService responseService)
        {
            this.responseService = responseService;
        }

        [HttpGet("{responseId}")]
        public async Task<object> GetResponse(string responseId)
        {
            bool responseIdParseSuccess = ObjectId.TryParse(responseId, out ObjectId responseObjectId);
            if (!responseIdParseSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Response Id"
                });

            ResponseViewModel response = await responseService.GetResponse(responseObjectId);
            if (response == null)
                return NotFound(new
                {
                    success = false,
                    message = "Incorrect Response Requested"
                });

            return Ok(new
            {
                success = true,
                response
            });
        }

        [HttpGet("form/{formId}")]
        public async Task<object> GetResponsesForForm(string formId)
        {
            bool formIdParseSuccess = ObjectId.TryParse(formId, out ObjectId formObjectId);
            if (!formIdParseSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Form Id"
                });

            IEnumerable<ResponseViewModel> responses = await responseService.GetResponsesForForm(formObjectId);
            return Ok(new
            {
                success = true,
                responses
            });
        }

        [HttpGet("user/{username}")]
        public async Task<object> GetResponsesCreatedBy(string username)
        {
            IEnumerable<ResponseViewModel> responses = await responseService.GetResponsesCreatedBy(username);
            return Ok(new
            {
                success = true,
                responses
            });
        }

        [HttpPost]
        public async Task<object> CreateResponse([FromBody]NewResponseViewModel response, [FromQuery]string formId)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Request Body"
                });

            bool formIdParseSuccess = ObjectId.TryParse(formId, out ObjectId formObjectId);
            if (!formIdParseSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Form Id"
                });

            try
            {
                ResponseViewModel savedResponse = await responseService.CreateResponse(response, formObjectId);
                return Created($"{Request.Host}{Request.PathBase}{Request.Path}",
                new
                {
                    success = true,
                    response = savedResponse
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

        [HttpPatch("{responseId}")]
        public async Task<object> UpdateResponse(string responseId,
            [FromBody]NewResponseViewModel response,
            [FromQuery]string formId)
        {
            bool formIdParseSuccess = ObjectId.TryParse(formId, out ObjectId formObjectId);
            bool responseIdParseSuccess = ObjectId.TryParse(responseId, out ObjectId responseObjectId);
            if (!formIdParseSuccess || !responseIdParseSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Id(s) Provided"
                });

            try
            {
                ResponseViewModel updatedResponse = await responseService.UpdateResponse(response, formObjectId, responseObjectId);
                return Created($"{Request.Host}{Request.PathBase}{Request.Path}",
                new
                {
                    success = true,
                    response = updatedResponse
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

        [HttpDelete("{responseId}")]
        public async Task<object> DeleteResponse(string responseId)
        {
            bool responseIdParseSuccess = ObjectId.TryParse(responseId, out ObjectId responseObjectId);
            if (!responseIdParseSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Response Id"
                });

            try
            {
                await responseService.DeleteResponse(responseObjectId);
                return Ok(new
                {
                    success = true,
                    message = "Response Successfully Deleted"
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

        [HttpDelete("form/{formId}")]
        public async Task<object> DeleteResponsesForForm(string formId)
        {
            bool formIdParseSuccess = ObjectId.TryParse(formId, out ObjectId formObjectId);
            if (!formIdParseSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Form Id"
                });

            try
            {
                long totalDeleted = await responseService.DeleteResponsesForForm(formObjectId);
                return Ok(new
                {
                    success = true,
                    message = "Responses Deleted",
                    totalDeleted
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

        [HttpDelete("user/{username}")]
        public async Task<object> DeleteResponsesCreatedBy(string username)
        {
            try
            {
                long totalDeleted = await responseService.DeleteResponsesCreatedBy(username);
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