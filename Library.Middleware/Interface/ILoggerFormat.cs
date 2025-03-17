namespace Library.Middleware.Interface;

public interface ILoggerFormat
{

    void Error(string message);

    void Information(string message);
}
