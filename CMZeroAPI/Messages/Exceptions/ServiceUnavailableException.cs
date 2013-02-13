namespace CMZero.API.Messages.Exceptions
{
	public class ServiceUnavailableException : BaseHttpException
    {
		public ServiceUnavailableException()
		{
		}

		public ServiceUnavailableException(string key) : base(key)
        {
        }

		public ServiceUnavailableException(string key, string message) : base(key, message)
		{
		}
    }
}