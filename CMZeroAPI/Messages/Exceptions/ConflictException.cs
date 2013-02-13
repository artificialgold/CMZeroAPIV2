namespace CMZero.API.Messages.Exceptions
{
	public class ConflictException : BaseHttpException
    {
		public ConflictException()
		{
		}

		public ConflictException(string key) : base(key)
        {
        }

		public ConflictException(string key, string message) : base(key, message)
		{
		}
    }
}