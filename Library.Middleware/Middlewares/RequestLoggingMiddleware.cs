using Library.Middleware.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Library.Middleware.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next, IRequestLogger requestLogger)
{
    public async Task Invoke(HttpContext context)
    {
        var startTime = DateTime.UtcNow.ToString("o");
        var stopwatch = Stopwatch.StartNew();
        Exception? exception = null;

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            exception = ex;
            context.Response.StatusCode = 500;
        }
        finally
        {
            stopwatch.Stop();

            switch (exception)
            {
                case null:
                    requestLogger.LogInfo(context, startTime, stopwatch.ElapsedMilliseconds);
                    break;
                default:
                    requestLogger.LogError(context, startTime, stopwatch.ElapsedMilliseconds, exception);
                    break;
            }
        }
    }
}
