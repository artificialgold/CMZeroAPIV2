namespace CMZero.API.Messages.Exceptions
{
	public class AccessForbiddenException : BaseHttpException
	{
		public AccessForbiddenException()
		{
		}

		public AccessForbiddenException(string key) : base (key)
		{
		}

		public AccessForbiddenException(string key, string message) : base(key, message)
		{
		}
	}
}