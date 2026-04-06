using Cart.Application.Cart.Interfaces;
using Catalog.Application.Product.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Orders.Application.Order.Interfaces;
using RetailHub.Services.Tests.Cart;
using RetailHub.SharedKernel.Application.Common.Abstractions;
using RetailHub.SharedKernel.Domain;
using OrderEntity = Orders.Domain.Order.Domain.Order;
using CartEntity = Cart.Domain.Cart.Domain.Cart;
using PlaceOrderAppCommand = Orders.Application.Order.Commands.PlaceOrder.PlaceOrderCommand;
using PlaceOrderCommandHandler = Orders.Application.Order.Commands.PlaceOrder.PlaceOrderCommandHandler;

namespace RetailHub.Services.Tests.Order.PlaceOrderCommand;

[TestFixture(Category = nameof(PlaceOrderCommandTests))]
public sealed class PlaceOrderCommandTests
{
    [Test]
    public async Task PlaceOrderCommand_CartNotFound_ReturnsNotFound()
    {
        var cartId = Guid.NewGuid();
        var command = new PlaceOrderAppCommand(cartId, UserId: null);

        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByIdWithItemsAsync(cartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CartEntity?)null);

        var orderRepo = new Mock<IOrderRepository>();
        var productRepo = new Mock<IProductReadRepository>();
        var userLookup = new Mock<IUserIdentityLookup>();

        var handler = new PlaceOrderCommandHandlerBuilder()
            .WithCartRepository(cartRepo)
            .WithOrderRepository(orderRepo)
            .WithProductReadRepository(productRepo)
            .WithUserIdentityLookup(userLookup)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(ResultCodes.NotFound);
        orderRepo.Verify(x => x.AddAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task PlaceOrderCommand_EmptyCart_ReturnsValidationError()
    {
        var cart = CartTestsHelper.CreateCart();
        var cartId = cart.Uid;
        var command = new PlaceOrderAppCommand(cartId, UserId: Guid.NewGuid());

        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByIdWithItemsAsync(cartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        var orderRepo = new Mock<IOrderRepository>();
        var productRepo = new Mock<IProductReadRepository>();
        productRepo
            .Setup(x => x.GetProductUidsByIdsAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<int, Guid>());
        var userLookup = new Mock<IUserIdentityLookup>();

        var handler = new PlaceOrderCommandHandlerBuilder()
            .WithCartRepository(cartRepo)
            .WithOrderRepository(orderRepo)
            .WithProductReadRepository(productRepo)
            .WithUserIdentityLookup(userLookup)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(ResultCodes.Validation);
    }

    [Test]
    public async Task PlaceOrderCommand_Valid_CreatesOrderAndClearsCart()
    {
        var productUid = Guid.NewGuid();
        var userUid = Guid.NewGuid();
        var cart = CartTestsHelper.CreateCartWithLine(1, 2, 12.5m);
        var cartId = cart.Uid;
        var command = new PlaceOrderAppCommand(cartId, userUid);

        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByIdWithItemsAsync(cartId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        var orderRepo = new Mock<IOrderRepository>();

        var productRepo = new Mock<IProductReadRepository>();
        productRepo
            .Setup(x => x.GetProductUidsByIdsAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<int, Guid> { { 1, productUid } });

        var userLookup = new Mock<IUserIdentityLookup>();
        userLookup
            .Setup(x => x.GetUserIdByUidAsync(userUid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(99);

        var handler = new PlaceOrderCommandHandlerBuilder()
            .WithCartRepository(cartRepo)
            .WithOrderRepository(orderRepo)
            .WithProductReadRepository(productRepo)
            .WithUserIdentityLookup(userLookup)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.CartId.Should().Be(cartId);
        result.Value.UserId.Should().Be(userUid);
        result.Value.Status.Should().Be(PlaceOrderCommandHandler.NewOrderStatus);
        result.Value.TotalAmount.Should().Be(25m);
        result.Value.Lines.Should().ContainSingle(l => l.ProductId == productUid && l.Quantity == 2);

        orderRepo.Verify(x => x.AddAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        orderRepo.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        cart.Items.Where(i => i.IsActive).Should().BeEmpty();
        cartRepo.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
