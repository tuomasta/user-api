using System;
using UserApi.ExceptionHandling;

namespace UserApi.Exceptions
{
    internal class ValidationException : Exception
    {
        public ValidationException(ValidationCode errorCode, string message)
            : base(message)
        {
            ValidationCode = errorCode;
        }

        public ValidationCode ValidationCode { get; }
    }
}