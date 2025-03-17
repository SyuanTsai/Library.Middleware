using Library.Middleware.Extensions;
using Library.Middleware.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace Library.Middleware.Common;

public class DefaultRequestLogger(ILogger<DefaultRequestLogger> logger) : IRequestLogger
{
    public void LogInfo(HttpContext context, string startTime, long elapsedMs)
    {
        logger.LogInformation(
            "Client IP: {ClientIp} | Request content type: {ContentType} | Request content length: {Length}" +
            "RouteTemplate: {RouteTemplate}" +
            "Request method: {Method} | Request path: {Path} | Request Query: {QueryParameter}" +
            "StatusCode: {ResponseStatusCode}" + "Response Status: {Status}" +
            "Start time(UTC): {Time}" +
            "Duration(ms): {Duration}" +
            "GroupId: {GroupId}" +
            "UserId: {UserId}" +
            "TokenExp: {TokenExp}",
            context.Connection.RemoteIpAddress?.ToString() ?? "Unknown", context.Request.ContentType, context.Request.ContentLength,
            context.Items.TryGetValue("RouteTemplate", out object? item) ? item?.ToString() : "",
            context.Request.Method, context.Request.Path, context.Request.QueryString.ToString(),
            context.Response?.StatusCode ?? 0, "Success",
            startTime,
            elapsedMs,
            context.User.GetGroupId(),
            context.User.GetUserId(),
            context.User.GetTokenExpiration()
        );
    }

    public void LogError(HttpContext context, string startTime, long elapsedMs, Exception exception)
    {
        logger.LogError(
            "Client IP: {ClientIp} | Request content type: {ContentType} | Request content length: {Length}" +
            "RouteTemplate: {RouteTemplate}" +
            "Request method: {Method} | Request path: {Path} | Request Query: {QueryParameter}" +
            "StatusCode: {ResponseStatusCode}" + "Response Status: {Status}" +
            "Start time(UTC): {Time}" +
            "Duration(ms): {Duration}" +
            "GroupId: {GroupId}" +
            "UserId: {UserId}" +
            "TokenExp: {TokenExp}" +
            "Error Message: {ErrorMessage}" +
            "StackTrace: {StackTrace}",
            context.Connection.RemoteIpAddress?.ToString() ?? "Unknown", context.Request.ContentType, context.Request.ContentLength,
            context.Items.TryGetValue("RouteTemplate", out object? item) ? item?.ToString() : "",
            context.Request.Method, context.Request.Path, context.Request.QueryString.ToString(),
            context.Response?.StatusCode ?? 0, "Error",
            startTime,
            elapsedMs,
            context.User.GetGroupId(),
            context.User.GetUserId(),
            context.User.GetTokenExpiration(),
            exception.Message,
            exception.StackTrace);
    }

}
