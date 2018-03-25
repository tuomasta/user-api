using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace AuthenticationApi.ErrorHandling
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception.ToString());
            context.Result = CreateResponce((dynamic) context.Exception, context);
        }

        /// <summary>
        /// Default base exception handler
        /// </summary>
        /// <param name="e"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private IActionResult CreateResponce(Exception e, ExceptionContext context){
            return new JsonResult(new {
                error = "Oops! Sorry something went wrong. Please contact to admin if error reoccurs" }
            ) {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
