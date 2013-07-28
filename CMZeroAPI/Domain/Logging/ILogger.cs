namespace CMZero.API.Domain.Logging
{
    public interface ILogger
    {
        void LogEvent(object logEvent);
    }
}