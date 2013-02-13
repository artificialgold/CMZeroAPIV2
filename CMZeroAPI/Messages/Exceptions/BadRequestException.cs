using System;

namespace CMZero.API.Messages.Exceptions
{
    public class BadRequestException : Exception
    {
        public ValidationErrors ValidationErrors { get; private set; }

        public BadRequestException(ValidationErrors validationErrors)
        {
            ValidationErrors = validationErrors;
        }
    }
}