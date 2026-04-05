using Cart.Application.Cart.Interfaces;
using Catalog.Application.Product.Interfaces;
using NUnit.Framework;
using FluentAssertions;
using Moq;
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
        var cartId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var cart = CartTestsHelper.CreateCartWithLine(cartId, productId, 4, 2m);
        var command = new UpdateQtyCommand(cartId, productId, 0);

        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByIdWithItemsAsync(cartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        var productRepo = new Mock<IProductReadRepository>();

        var handler = new UpdateCartItemQuantityCommandHandlerBuilder()
            .WithCartRepository(cartRepo)
            .WithProductReadRepository(productRepo)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Lines.Should().BeEmpty();
        productRepo.Verify(
            x => x.GetActiveProductByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    public async Task UpdateCartItemQuantityCommand_ProductNotInCart_ReturnsNotFound()
    {
        var cartId = Guid.NewGuid();
        var missingProductId = Guid.NewGuid();
        var cart = CartTestsHelper.CreateCart(id: cartId);
        var command = new UpdateQtyCommand(cartId, missingProductId, 2);

        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByIdWithItemsAsync(cartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        var productRepo = new Mock<IProductReadRepository>();
        productRepo.Setup(x => x.GetActiveProductByIdAsync(missingProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CartTestsHelper.CreateProduct(id: missingProductId));

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
        var cartId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var cart = CartTestsHelper.CreateCartWithLine(cartId, productId, 1, 10m);
        var product = CartTestsHelper.CreateProduct(id: productId, price: 10m);
        var command = new UpdateQtyCommand(cartId, productId, 5);

        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByIdWithItemsAsync(cartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        var productRepo = new Mock<IProductReadRepository>();
        productRepo.Setup(x => x.GetActiveProductByIdAsync(productId, It.IsAny<CancellationToken>()))
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
