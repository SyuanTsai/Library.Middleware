using Microsoft.AspNetCore.Http;
using System;

namespace Library.Middleware.Interface;

public interface IRequestLogger
{
    void LogInfo(HttpContext context, string startTime, long elapsedMs);
    void LogError(HttpContext context, string startTime, long elapsedMs, Exception exception);
}
