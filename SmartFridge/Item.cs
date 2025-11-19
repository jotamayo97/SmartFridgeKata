namespace SmartFridge;

public record Item
{
    public readonly ItemState state;
    public object Name { get;}
    public DateTime Expiry { get; set; }
    public Item(string name, DateTime expiry, ItemState state)
    {
        this.Name = name;
        this.Expiry = expiry;
        this.state = state;
    }
}