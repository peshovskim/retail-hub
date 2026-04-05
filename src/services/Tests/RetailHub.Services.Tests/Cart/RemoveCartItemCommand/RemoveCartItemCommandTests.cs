using Cart.Application.Cart.Interfaces;
using Catalog.Application.Product.Interfaces;
using NUnit.Framework;
using FluentAssertions;
using Moq;
using RetailHub.SharedKernel.Domain;
using RemoveItemCommand = Cart.Application.Cart.Commands.RemoveCartItem.RemoveCartItemCommand;
using CartEntity = Cart.Domain.Cart.Domain.Cart;

namespace RetailHub.Services.Tests.Cart.RemoveCartItemCommand;

[TestFixture(Category = nameof(RemoveCartItemCommandTests))]
public sealed class RemoveCartItemCommandTests
{
    [Test]
    public async Task RemoveCartItemCommand_CartNotFound_ReturnsNotFound()
    {
        var cartId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new RemoveItemCommand(cartId, productId);

        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByIdWithItemsAsync(cartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CartEntity?)null);

        var productRepo = new Mock<IProductReadRepository>();

        var handler = new RemoveCartItemCommandHandlerBuilder()
            .WithCartRepository(cartRepo)
            .WithProductReadRepository(productRepo)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(ResultCodes.NotFound);
    }

    [Test]
    public async Task RemoveCartItemCommand_Valid_RemovesLineAndPersists()
    {
        var cartId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var cart = CartTestsHelper.CreateCartWithLine(cartId, productId, 2, 4.5m);
        var command = new RemoveItemCommand(cartId, productId);

        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByIdWithItemsAsync(cartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        var productRepo = new Mock<IProductReadRepository>();

        var handler = new RemoveCartItemCommandHandlerBuilder()
            .WithCartRepository(cartRepo)
            .WithProductReadRepository(productRepo)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Lines.Should().BeEmpty();
        result.Value.Subtotal.Should().Be(0m);
        result.Value.ItemCount.Should().Be(0);
        cartRepo.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
