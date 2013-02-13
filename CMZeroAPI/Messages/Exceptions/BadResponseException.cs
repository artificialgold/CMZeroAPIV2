using System;

namespace CMZero.API.Messages.Exceptions
{
    public class BadResponseException : Exception
    {
        public BadResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}