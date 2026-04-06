using Catalog.Application.Product.Responses;
using CartEntity = Cart.Domain.Cart.Domain.Cart;

namespace RetailHub.Services.Tests.Cart;

/// <summary>Factory methods for cart command tests.</summary>
public static class CartTestsHelper
{
    public static CartEntity CreateCart(
        DateTime? createdOn = null,
        int? userId = null,
        string? anonymousKey = "test-anon-key")
    {
        return CartEntity.Create(
            createdOn ?? DateTime.UtcNow,
            userId,
            anonymousKey);
    }

    /// <summary>Adds a line using domain rules; throws if the domain rejects the add.</summary>
    public static CartEntity CreateCartWithLine(
        int productId,
        int quantity,
        decimal unitPrice,
        string? anonymousKey = "test-anon-key")
    {
        var cart = CreateCart(anonymousKey: anonymousKey);
        var add = cart.AddOrUpdateItem(productId, quantity, unitPrice, DateTime.UtcNow);
        if (add.IsFailure)
        {
            throw new InvalidOperationException(add.Error!.Message);
        }

        return cart;
    }

    public static ProductResponse CreateProduct(
        int productId = 1,
        Guid? id = null,
        int categoryId = 1,
        string name = "Test product",
        string slug = "test-product",
        string sku = "SKU-1",
        decimal price = 19.99m,
        string shortDescription = "Short",
        string description = "Description",
        string? categoryName = "Cat")
    {
        return new ProductResponse(
            productId,
            id ?? Guid.NewGuid(),
            categoryId,
            name,
            slug,
            sku,
            price,
            shortDescription,
            description,
            categoryName);
    }
}
