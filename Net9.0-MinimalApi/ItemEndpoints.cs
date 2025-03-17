using Library.DemoService;
using Microsoft.AspNetCore.Mvc;

namespace Net9._0_MinimalApi;

public static  class ItemEndpoints
{
    public static void RegisterItemEndpoints(this IEndpointRouteBuilder app, IItemService itemService)
    {
        var pageSize = 5; // 預設分頁大小

        // 取得特定 ID 的項目
        app.MapGet("/items/{id:guid}", (Guid id) =>
            {
                var item = itemService.GetItemById(id);
                return item is not null ? Results.Ok(item) : Results.NotFound();
            })
            .WithName("GetItemById")
            .WithOpenApi();

        // 取得分頁清單
        app.MapGet("/items/list/{page:int}", (int page) =>
            {
                var pagedItems = itemService.GetPagedItems(page, pageSize);
                return Results.Ok(pagedItems);
            })
            .WithName("GetItemsList")
            .WithOpenApi();

        // 新增項目
        app.MapPost("/items", ([FromBody] Item item) =>
            {
                var newItem = itemService.CreateItem(item);
                return Results.Created($"/items/{newItem.Id}", newItem);
            })
            .WithName("CreateItem")
            .WithOpenApi();

        // 更新項目
        app.MapPut("/items/{id:guid}", (Guid id, [FromBody] Item updatedItem) =>
            {
                return itemService.UpdateItem(id, updatedItem)
                    ? Results.NoContent()
                    : Results.NotFound();
            })
            .WithName("UpdateItem")
            .WithOpenApi();

        // 刪除項目
        app.MapDelete("/items/{id:guid}", (Guid id) =>
            {
                return itemService.DeleteItem(id) ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteItem")
            .WithOpenApi();
    }
}
