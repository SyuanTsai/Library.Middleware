using Library.DemoService;
using Library.Middleware.ActionFilter;
using Microsoft.AspNetCore.Mvc;

namespace Net9._0_MinimalApi;

public static class ItemEndpoints
{
    public static void RegisterItemEndpoints(this IEndpointRouteBuilder app)
    {
        var itemService = app.ServiceProvider.GetRequiredService<IItemService>();
        var pageSize = 5; // 預設分頁大小

        var group = app.MapGroup("/items")
            .AddEndpointFilter<RouteTemplateFilter>();

        // 取得特定 ID 的項目
        group.MapGet("/{id:guid}", (Guid id) =>
            {
                var item = itemService.GetItemById(id);
                return item is not null ? Results.Ok(item) : Results.NotFound();
            })
            .WithName("GetItemById")
            .WithOpenApi();

        // 取得分頁清單
        group.MapGet("/list/{page:int}", (int page) =>
            {
                var pagedItems = itemService.GetPagedItems(page, pageSize);
                return Results.Ok(pagedItems);
            })
            .WithName("GetItemsList")
            .WithOpenApi();

        // 新增項目
        group.MapPost("", ([FromBody] Item item) =>
            {
                var newItem = itemService.CreateItem(item);
                return Results.Created($"/items/{newItem.Id}", newItem);
            })
            .WithName("CreateItem")
            .WithOpenApi();

        // 更新項目
        group.MapPut("/{id:guid}",
                (Guid id, [FromBody] Item updatedItem) => itemService.UpdateItem(id, updatedItem)
                    ? Results.NoContent()
                    : Results.NotFound())
            .WithName("UpdateItem")
            .WithOpenApi();

        // 刪除項目
        group.MapDelete("/{id:guid}",
                (Guid id) => itemService.DeleteItem(id) ? Results.NoContent() : Results.NotFound())
            .WithName("DeleteItem")
            .WithOpenApi();
    }

}
