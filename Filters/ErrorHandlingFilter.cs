using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Forms.Filters
{
    public class ErrorHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            Console.WriteLine(exception.Message);

            context.ExceptionHandled = true;
        }
    }
}