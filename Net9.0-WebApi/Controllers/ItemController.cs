using Library.DemoService;
using Microsoft.AspNetCore.Mvc;

namespace Net9._0_WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemController(IItemService itemService) : ControllerBase
{
    private const int pageSize = 5; // 這裡示範固定取 5 筆，可依需求修改或改成動態參數

    // 1. 取得特定 ID 的項目
    [HttpGet("{id:guid}")]
    public IActionResult GetItemById(Guid id)
    {
        var item = itemService.GetItemById(id);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    // 2. 取得分頁清單
    [HttpGet("list/{page:int}")]
    public IActionResult GetItemsList(int page)
    {
        var pagedItems = itemService.GetPagedItems(page, pageSize);
        return Ok(pagedItems);
    }

    // 3. 新增項目
    [HttpPost]
    public IActionResult CreateItem([FromBody] Item item)
    {
        var newItem = itemService.CreateItem(item);
        // 回傳的路徑可以視實際路由而定，此處使用 /items/{newItem.Id} 作示範
        return Created($"/items/{newItem.Id}", newItem);
    }

    // 4. 更新項目
    [HttpPut("{id:guid}")]
    public IActionResult UpdateItem(Guid id, [FromBody] Item updatedItem)
    {
        var success = itemService.UpdateItem(id, updatedItem);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }

    // 5. 刪除項目
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteItem(Guid id)
    {
        var success = itemService.DeleteItem(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}