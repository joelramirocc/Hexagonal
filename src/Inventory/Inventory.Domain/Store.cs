using System;
using System.Collections.Generic;

namespace Inventory.Domain;

/// <summary>
/// Representa una tienda que administra su inventario de productos.
/// </summary>
public class Store
{
    private readonly Dictionary<Guid, Product> _products = new();

    public Guid Id { get; }
    public string Name { get; private set; }

    public Store(Guid id, string name)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("El identificador de la tienda no puede ser vacío.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre de la tienda es obligatorio.", nameof(name));
        }

        Id = id;
        Name = name;
    }

    public IReadOnlyCollection<Product> Products => _products.Values;

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("El nombre de la tienda es obligatorio.", nameof(newName));
        }

        Name = newName;
    }

    public void AddProduct(Product product)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        if (_products.ContainsKey(product.Id))
        {
            throw new InvalidOperationException($"El producto con Id {product.Id} ya existe en la tienda.");
        }

        _products.Add(product.Id, product);
    }

    public void RemoveProduct(Guid productId)
    {
        if (!_products.Remove(productId))
        {
            throw new KeyNotFoundException($"No se encontró el producto con Id {productId} en la tienda.");
        }
    }

    public Product GetProduct(Guid productId)
    {
        if (!_products.TryGetValue(productId, out var product))
        {
            throw new KeyNotFoundException($"No se encontró el producto con Id {productId} en la tienda.");
        }

        return product;
    }

    public bool TryGetProduct(Guid productId, out Product? product)
    {
        var found = _products.TryGetValue(productId, out var value);
        product = value;
        return found;
    }
}
