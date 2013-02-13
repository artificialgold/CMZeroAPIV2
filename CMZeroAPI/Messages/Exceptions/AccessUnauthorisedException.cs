namespace CMZero.API.Messages.Exceptions
{
	public class AccessUnauthorisedException : BaseHttpException
    {
		public AccessUnauthorisedException()
		{
		}

		public AccessUnauthorisedException(string key) : base(key)
        {
        }

		public AccessUnauthorisedException(string key, string message) : base(key, message)
		{
		}
    }
}