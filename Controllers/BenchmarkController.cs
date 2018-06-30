using Flurl;
using Flurl.Http;
using Forms.Generators;
using Forms.Models.BenchmarkResultModels;
using Forms.Models.DBModels;
using Forms.Models.NewModels;
using Forms.Models.ResponseModels;
using Forms.ServiceInterfaces;
using Forms.Utils;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Forms.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BenchmarkController : Controller
    {
        FormGenerator formGenerator;
        ResponseGenerator responseGenerator;
        private readonly IFormService formService;

        public BenchmarkController(IFormService formService)
        {
            formGenerator = new FormGenerator();
            responseGenerator = new ResponseGenerator();
            this.formService = formService;
        }

        [HttpGet("form")]
        public async Task<object> CreateFormBenchmark([FromQuery] string fieldCount, [FromQuery] string totalForms)
        {
            bool isValidFieldCount = int.TryParse(fieldCount, out int fieldCountValue);
            if (!isValidFieldCount)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Field Count"
                });

            bool isValidTotalForms = int.TryParse(totalForms, out int totalFormsValue);
            if (!isValidTotalForms)
                totalFormsValue = 1;


            List<FormBenchmarkResponseModel> forms = new List<FormBenchmarkResponseModel>();
            Stopwatch watch;

            try
            {
                for (int i = 0; i < totalFormsValue; i++)
                {
                    NewFormViewModel newForm = formGenerator.GenerateRandomForm(fieldCountValue);
                    watch = Stopwatch.StartNew();

                    FormObjectViewModel form = await $"{Constants.BASE_URL}"
                        .AppendPathSegment("form")
                        .PostJsonAsync(newForm)
                        .ReceiveJson<FormObjectViewModel>();

                    watch.Stop();

                    forms.Add(new FormBenchmarkResponseModel
                    {
                        formId = form.Id.ToString(),
                        timeElapsed = watch.ElapsedMilliseconds
                    });
                }

                return Ok(new
                {
                    success = true,
                    totalFormsCreated = forms.Count,
                    forms
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new JsonResult(new
                {
                    success = false,
                    totalFormsCreated = forms.Count,
                    forms,
                    message = e.Message
                });
            }
        }

        [HttpGet("response")]
        public async Task<object> CreateResponseBenchmark([FromQuery] string formId, [FromQuery] string incorrectRatio)
        {
            bool formIdParseSuccess = ObjectId.TryParse(formId, out ObjectId formIdObject);
            if (!formIdParseSuccess)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Form Id"
                });


            bool incorrectRatioParseSuccess = float.TryParse(incorrectRatio, out float incorrectRatioValue);
            if (!incorrectRatioParseSuccess)
                incorrectRatioValue = 0;

            try
            {
                FormResponseModel formResponse = await $"{Constants.BASE_URL}"
                    .AppendPathSegment("form")
                    .AppendPathSegment($"{formId}")
                    .GetJsonAsync<FormResponseModel>();



                NewResponseViewModel newResponse = responseGenerator.GenerateRandomResponse(
                    formResponse.form, incorrectRatioValue);

                var watch = Stopwatch.StartNew();

                ResponseViewModel response = await $"{Constants.BASE_URL}"
                    .AppendPathSegment("response")
                    .PostJsonAsync(newResponse)
                    .ReceiveJson<ResponseViewModel>();

                watch.Stop();

                return Ok(new
                {
                    success = true,
                    response,
                    timeElapsed = watch.ElapsedMilliseconds
                });
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return new JsonResult(new
                {
                    success = false,
                    message = e.Message
                });
            }
        }

        [HttpGet("responses")]
        public async Task<object> CreateMultipleResponseBenchmark([FromBody] FormIdMultipleModel formIds)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid Params Requested"
                });

            throw new NotImplementedException();
        }
    }
}