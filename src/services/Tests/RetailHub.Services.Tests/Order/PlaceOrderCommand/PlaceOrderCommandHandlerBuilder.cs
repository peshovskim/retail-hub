using Cart.Application.Cart.Interfaces;
using Moq;
using Orders.Application.Order.Commands.PlaceOrder;
using Orders.Application.Order.Interfaces;

namespace RetailHub.Services.Tests.Order.PlaceOrderCommand;

public sealed class PlaceOrderCommandHandlerBuilder
{
    private Mock<ICartRepository> _cartRepository = new();
    private Mock<IOrderRepository> _orderRepository = new();

    public PlaceOrderCommandHandlerBuilder WithCartRepository(Mock<ICartRepository> cartRepository)
    {
        _cartRepository = cartRepository;
        return this;
    }

    public PlaceOrderCommandHandlerBuilder WithOrderRepository(Mock<IOrderRepository> orderRepository)
    {
        _orderRepository = orderRepository;
        return this;
    }

    public PlaceOrderCommandHandler Build() =>
        new(_cartRepository.Object, _orderRepository.Object);
}
