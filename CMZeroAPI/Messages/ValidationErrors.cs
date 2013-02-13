using System.Collections.Generic;

namespace CMZero.API.Messages
{
    public class ValidationErrors
    {
        private IList<Error> _errors = new List<Error>();

        public IList<Error> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }

        public void AddError(string key, string message)
        {
            _errors.Add(new Error { Key = key, Message = message });
        }
    }
}