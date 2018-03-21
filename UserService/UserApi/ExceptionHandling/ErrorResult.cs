using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using UserApi.Exceptions;

namespace UserApi.ExceptionHandling
{
    internal class ErrorResult : IHttpActionResult
    {
        public ErrorResult(HttpStatusCode statusCode, string message, HttpRequestMessage httpRequest, ValidationCode errorCode = ValidationCode.Unknown)
        {
            Request = httpRequest;
            StatusCode = statusCode;
            Message = message;
            ErrorCode = errorCode;
        }

        public HttpRequestMessage Request { get; }
        public HttpStatusCode StatusCode { get; }
        public string Message { get; }
        public ValidationCode ErrorCode { get; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            => Task.FromResult(Request.CreateResponse(StatusCode, new { Message, ErrorCode }));
    }


    internal enum ValidationCode
    {
        Unknown = 0,
        DuplicateEmail,
    }
}