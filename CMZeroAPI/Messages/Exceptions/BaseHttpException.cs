using System;

namespace CMZero.API.Messages.Exceptions
{
	public abstract class BaseHttpException : Exception
	{
		private readonly string _key;

		protected BaseHttpException()
		{
			_key = GetType().Name.Substring(0, GetType().Name.Length - 9);
		}

		protected BaseHttpException(string key) : this(key, string.Empty)
		{
		}

		protected BaseHttpException(string key, string message) : base(message)
		{
			_key = key;
		}

		public string Key
		{
			get { return _key; }
		}

		public void ThrowAs<T>() where T : Exception, new()
		{
			if (typeof(T).Name == _key + "Exception")
			{
				throw new T();
			}
		}
	}
}