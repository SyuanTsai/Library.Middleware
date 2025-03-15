using Microsoft.AspNetCore.Mvc.Filters;

namespace Library.Middleware.ActionFilter;

/// <summary>
/// Attach routing template info to httpContext. This is extracted by logging middleware for analytic purposes
/// </summary>
public class RouteTemplateFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Items.Add("RouteTemplate", context.ActionDescriptor.AttributeRouteInfo.Template);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
