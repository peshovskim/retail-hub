using Cart.Application.Cart.Interfaces;
using Catalog.Application.Product.Interfaces;
using NUnit.Framework;
using FluentAssertions;
using Moq;
using RetailHub.Services.Tests.Cart;
using RetailHub.SharedKernel.Domain;using AddItemCommand = Cart.Application.Cart.Commands.AddCartItem.AddCartItemCommand;
using CartEntity = Cart.Domain.Cart.Domain.Cart;

namespace RetailHub.Services.Tests.Cart.AddCartItemCommand;

[TestFixture(Category = nameof(AddCartItemCommandTests))]
public sealed class AddCartItemCommandTests
{
    [Test]
    public async Task AddCartItemCommand_CartNotFound_ReturnsNotFound()
    {
        var cartId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new AddItemCommand(cartId, productId, 1);

        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByIdWithItemsAsync(cartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CartEntity?)null);

        var productRepo = new Mock<IProductReadRepository>();

        var handler = new AddCartItemCommandHandlerBuilder()
            .WithCartRepository(cartRepo)
            .WithProductReadRepository(productRepo)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(ResultCodes.NotFound);
        cartRepo.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task AddCartItemCommand_ProductNotFound_ReturnsNotFound()
    {
        var productId = Guid.NewGuid();
        var cart = CartTestsHelper.CreateCart();
        var cartId = cart.Uid;
        var command = new AddItemCommand(cartId, productId, 2);

        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByIdWithItemsAsync(cartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        var productRepo = new Mock<IProductReadRepository>();
        productRepo.Setup(x => x.GetActiveProductByUidAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Catalog.Application.Product.Responses.ProductResponse?)null);

        var handler = new AddCartItemCommandHandlerBuilder()
            .WithCartRepository(cartRepo)
            .WithProductReadRepository(productRepo)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(ResultCodes.NotFound);
        cartRepo.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task AddCartItemCommand_Valid_AddsLineAndReturnsCart()
    {
        var productId = Guid.NewGuid();
        var cart = CartTestsHelper.CreateCart();
        var cartId = cart.Uid;
        var command = new AddItemCommand(cartId, productId, 3);
        var product = CartTestsHelper.CreateProduct(id: productId, price: 5m);

        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByIdWithItemsAsync(cartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        var productRepo = new Mock<IProductReadRepository>();
        productRepo.Setup(x => x.GetActiveProductByUidAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        productRepo.Setup(x => x.GetActiveProductByInternalIdAsync(product.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var handler = new AddCartItemCommandHandlerBuilder()
            .WithCartRepository(cartRepo)
            .WithProductReadRepository(productRepo)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Subtotal.Should().Be(15m);
        result.Value.ItemCount.Should().Be(3);
        result.Value.Lines.Should().ContainSingle(l => l.ProductId == productId && l.Quantity == 3);
        cartRepo.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        productRepo.Verify(x => x.GetActiveProductByUidAsync(productId, It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }
}
