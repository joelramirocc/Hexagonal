using System;

namespace Inventory.Domain;

/// <summary>
/// Representa un producto dentro del inventario de una tienda.
/// </summary>
public class Product
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public string Sku { get; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }

    public Product(Guid id, string name, string sku, decimal price, int quantity)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("El identificador del producto no puede ser vacío.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre del producto es obligatorio.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new ArgumentException("El SKU del producto es obligatorio.", nameof(sku));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "El precio no puede ser negativo.");
        }

        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "La cantidad no puede ser negativa.");
        }

        Id = id;
        Name = name;
        Sku = sku;
        Price = price;
        Quantity = quantity;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("El nombre del producto es obligatorio.", nameof(newName));
        }

        Name = newName;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(newPrice), "El precio no puede ser negativo.");
        }

        Price = newPrice;
    }

    public void AdjustStock(int quantityDelta)
    {
        var newQuantity = Quantity + quantityDelta;
        if (newQuantity < 0)
        {
            throw new InvalidOperationException("No es posible dejar la existencia del producto en negativo.");
        }

        Quantity = newQuantity;
    }

    public void SetStock(int quantity)
    {
        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "La cantidad no puede ser negativa.");
        }

        Quantity = quantity;
    }
}
