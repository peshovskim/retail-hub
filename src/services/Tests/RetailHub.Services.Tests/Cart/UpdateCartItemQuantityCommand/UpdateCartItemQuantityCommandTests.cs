using Cart.Application.Cart.Interfaces;
using Catalog.Application.Product.Interfaces;
using NUnit.Framework;
using FluentAssertions;
using Moq;
using RetailHub.Services.Tests.Cart;
using RetailHub.SharedKernel.Domain;
using UpdateQtyCommand = Cart.Application.Cart.Commands.UpdateCartItemQuantity.UpdateCartItemQuantityCommand;
using CartEntity = Cart.Domain.Cart.Domain.Cart;

namespace RetailHub.Services.Tests.Cart.UpdateCartItemQuantityCommand;

[TestFixture(Category = nameof(UpdateCartItemQuantityCommandTests))]
public sealed class UpdateCartItemQuantityCommandTests
{
    [Test]
    public async Task UpdateCartItemQuantityCommand_CartNotFound_ReturnsNotFound()
    {
        var cartId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new UpdateQtyCommand(cartId, productId, 5);

        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByIdWithItemsAsync(cartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CartEntity?)null);

        var productRepo = new Mock<IProductReadRepository>();

        var handler = new UpdateCartItemQuantityCommandHandlerBuilder()
            .WithCartRepository(cartRepo)
            .WithProductReadRepository(productRepo)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(ResultCodes.NotFound);
    }

    [Test]
    public async Task UpdateCartItemQuantityCommand_QuantityZero_RemovesLine()
    {
        var productUid = Guid.NewGuid();
        var product = CartTestsHelper.CreateProduct(productId: 5, id: productUid);
        var cart = CartTestsHelper.CreateCartWithLine(5, 4, 2m);
        var cartId = cart.Uid;
        var command = new UpdateQtyCommand(cartId, productUid, 0);

        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByIdWithItemsAsync(cartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        var productRepo = new Mock<IProductReadRepository>();
        productRepo.Setup(x => x.GetActiveProductByUidAsync(productUid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var handler = new UpdateCartItemQuantityCommandHandlerBuilder()
            .WithCartRepository(cartRepo)
            .WithProductReadRepository(productRepo)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Lines.Should().BeEmpty();
    }

    [Test]
    public async Task UpdateCartItemQuantityCommand_ProductNotInCart_ReturnsNotFound()
    {
        var missingProductUid = Guid.NewGuid();
        var cart = CartTestsHelper.CreateCart();
        var cartId = cart.Uid;
        var command = new UpdateQtyCommand(cartId, missingProductUid, 2);

        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByIdWithItemsAsync(cartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        var productRepo = new Mock<IProductReadRepository>();
        productRepo.Setup(x => x.GetActiveProductByUidAsync(missingProductUid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CartTestsHelper.CreateProduct(productId: 99, id: missingProductUid));

        var handler = new UpdateCartItemQuantityCommandHandlerBuilder()
            .WithCartRepository(cartRepo)
            .WithProductReadRepository(productRepo)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(ResultCodes.NotFound);
    }

    [Test]
    public async Task UpdateCartItemQuantityCommand_Valid_UpdatesQuantity()
    {
        var productUid = Guid.NewGuid();
        var cart = CartTestsHelper.CreateCartWithLine(1, 1, 10m);
        var cartId = cart.Uid;
        var product = CartTestsHelper.CreateProduct(productId: 1, id: productUid, price: 10m);
        var command = new UpdateQtyCommand(cartId, productUid, 5);

        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByIdWithItemsAsync(cartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        var productRepo = new Mock<IProductReadRepository>();
        productRepo.Setup(x => x.GetActiveProductByUidAsync(productUid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var handler = new UpdateCartItemQuantityCommandHandlerBuilder()
            .WithCartRepository(cartRepo)
            .WithProductReadRepository(productRepo)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.ItemCount.Should().Be(5);
        result.Value.Subtotal.Should().Be(50m);
    }
}
