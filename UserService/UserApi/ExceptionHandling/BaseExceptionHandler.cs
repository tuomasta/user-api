using System;
using System.Net;
using System.Web.Http.ExceptionHandling;
using UserApi.Exceptions;

namespace UserApi.ExceptionHandling
{
    /// <summary>
    /// Exception handler to handle all exception if not handled by another handler
    /// </summary>
    public class BaseExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var responce = GetResponce((dynamic)context.Exception, context);

            if (responce != null) context.Result = responce;
            else base.Handle(context);
        }

        private ErrorResult GetResponce(Exception exception, ExceptionHandlerContext context)
            => new ErrorResult(
                HttpStatusCode.InternalServerError,
                "Oops! Sorry something went wrong. Please contact to admin if error reoccurs",
                context.Request);

        private ErrorResult GetResponce(ValidationException exception, ExceptionHandlerContext context)
            => new ErrorResult(
                HttpStatusCode.BadRequest,
                exception.Message,
                context.Request,
                exception.ValidationCode);
    }
}