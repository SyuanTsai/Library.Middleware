using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace Library.Middleware.ActionFilter;

/// <summary>
/// Attach routing template info to httpContext. This is extracted by logging middleware for analytic purposes
/// </summary>
public class RouteTemplateFilter : IActionFilter, IEndpointFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Items.Add("RouteTemplate", context.ActionDescriptor.AttributeRouteInfo?.Template);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var endpoint = context.HttpContext.GetEndpoint();

        // 若是傳統 Route-based Endpoint (RouteEndpoint)
        if (endpoint is not RouteEndpoint routeEndpoint)
            return await next(context);

        // 取得路由樣板 (e.g. "/sdk/hello/{id}")
        var routePattern = routeEndpoint.RoutePattern.RawText;

        // 放到 HttpContext.Items
        context.HttpContext.Items["RouteTemplate"] = routePattern;

        // 若要繼續呼叫後續的 Filter 或 Endpoint
        return await next(context);
    }
}

