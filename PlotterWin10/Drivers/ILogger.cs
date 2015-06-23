namespace Drivers
{
    public enum LogType
    {
        Info,
        Success,
        Warning,
        Error
    }

    public interface ILogger
    {
        void WriteLn(string message, LogType logType = LogType.Info);
    }
}
