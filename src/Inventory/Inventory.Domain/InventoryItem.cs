namespace Inventory.Domain;

public sealed class InventoryItem
{
    public InventoryItem(Guid id, string sku, int quantity)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new ArgumentException("SKU must be provided", nameof(sku));
        }

        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity cannot be negative");
        }

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Sku = sku.Trim().ToUpperInvariant();
        Quantity = quantity;
    }

    public Guid Id { get; }

    public string Sku { get; }

    public int Quantity { get; private set; }

    public void AdjustQuantity(int delta)
    {
        var newQuantity = Quantity + delta;
        if (newQuantity < 0)
        {
            throw new InvalidOperationException("Cannot reduce inventory below zero");
        }

        Quantity = newQuantity;
    }
}
