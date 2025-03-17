using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Library.Middleware;

public partial class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger = null)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        // 開始計時
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            // 繼續執行管道中的下一個中間件
            await _next(context);
        }
        finally
        {
            // 停止計時
            stopwatch.Stop();
            
            // 記錄執行時間
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            
            // 取得 Query Parameters
            string queryParameters = context.Request.QueryString.ToString();

            string? routeTemplate = context.Items.TryGetValue("RouteTemplate", out object? item) ? item?.ToString() : "";
		

            _logger?.LogInformation(
                "Request {Method} {Path} completed in {ElapsedMilliseconds}ms. Query: {QueryParams}, Route: {RouteTemplate}, Status: {StatusCode}",
                context.Request.Method,
                context.Request.Path,
                elapsedMs,
                queryParameters,
                routeTemplate,
                context.Response.StatusCode);
        }
    }
}
