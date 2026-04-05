using Catalog.Application.Product.Responses;
using CartEntity = Cart.Domain.Cart.Domain.Cart;

namespace RetailHub.Services.Tests.Cart;

/// <summary>Factory methods for cart command tests.</summary>
public static class CartTestsHelper
{
    public static CartEntity CreateCart(
        Guid? id = null,
        DateTime? createdOn = null,
        Guid? userId = null,
        string? anonymousKey = "test-anon-key")
    {
        return CartEntity.Create(
            id ?? Guid.NewGuid(),
            createdOn ?? DateTime.UtcNow,
            userId,
            anonymousKey);
    }

    /// <summary>Adds a line using domain rules; throws if the domain rejects the add.</summary>
    public static CartEntity CreateCartWithLine(
        Guid cartId,
        Guid productId,
        int quantity,
        decimal unitPrice,
        string? anonymousKey = "test-anon-key")
    {
        var cart = CreateCart(id: cartId, anonymousKey: anonymousKey);
        var add = cart.AddOrUpdateItem(productId, quantity, unitPrice, DateTime.UtcNow, Guid.NewGuid);
        if (add.IsFailure)
        {
            throw new InvalidOperationException(add.Error!.Message);
        }

        return cart;
    }

    public static ProductResponse CreateProduct(
        Guid? id = null,
        Guid? categoryId = null,
        string name = "Test product",
        string slug = "test-product",
        string sku = "SKU-1",
        decimal price = 19.99m,
        string shortDescription = "Short",
        string description = "Description",
        string? categoryName = "Cat")
    {
        return new ProductResponse(
            id ?? Guid.NewGuid(),
            categoryId ?? Guid.NewGuid(),
            name,
            slug,
            sku,
            price,
            shortDescription,
            description,
            categoryName);
    }
}
