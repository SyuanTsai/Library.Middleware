namespace Library.DemoService;

public interface IItemService
{
    IEnumerable<Item> GetPagedItems(int page, int pageSize);
    Item? GetItemById(Guid id);
    Item CreateItem(Item item);
    bool UpdateItem(Guid id, Item updatedItem);
    bool DeleteItem(Guid id);
}
