using Library.Middleware.ActionFilter;
using Library.Middleware.Common;
using Library.Middleware.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Library.Middleware;

public static class MiddlewareDependencyInjection
{
    public static IServiceCollection AddSdkControllers(this IServiceCollection services)
    {
        // 1) 如果要在全域加 SDK Filter，可直接用 Configure<MvcOptions>。
        services.Configure<MvcOptions>(options =>
        {
            // 例如全域掛一個由 SDK 提供的 Filter
            options.Filters.Add(new RouteTemplateFilter());
        });

        // 2) 註冊一個自訂的 IConfigureOptions<MvcOptions>，動態把 SDK 內的 Controllers 加到 ApplicationPartManager
        services.AddTransient<IConfigureOptions<MvcOptions>, SdkApplicationPartManagerSetup>();

        return services;
    }

    public static IServiceCollection AddMiddleware(this IServiceCollection services)
    {
        services.AddSingleton<IRequestLogger, DefaultRequestLogger>();
        return services;
    }

    public static IApplicationBuilder UseMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestLoggingMiddleware>();
        return app;
    }
}
