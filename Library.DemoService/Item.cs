namespace Library.DemoService;

public record Item
{
    public Guid Id { get; set; }
    public string Name { get; init; } = default!;
    public int Value { get; init; }
}
