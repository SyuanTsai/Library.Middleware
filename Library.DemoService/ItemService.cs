namespace Library.DemoService;

public class ItemService : IItemService
{
    private readonly Dictionary<Guid, Item> _items = new Dictionary<Guid, Item>();

    public IEnumerable<Item> GetPagedItems(int page, int pageSize)
    {
        return _items.Values
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
    }

    public Item? GetItemById(Guid id)
    {
        _items.TryGetValue(id, out var item);
        return item;
    }

    public Item CreateItem(Item item)
    {
        item.Id = Guid.NewGuid();
        _items[item.Id] = item;
        return item;
    }

    public bool UpdateItem(Guid id, Item updatedItem)
    {
        if (!_items.ContainsKey(id))
            return false;

        updatedItem.Id = id;
        _items[id] = updatedItem;
        return true;
    }

    public bool DeleteItem(Guid id)
    {
        return _items.Remove(id);
    }
}
