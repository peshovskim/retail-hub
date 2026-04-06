using Cart.Application.Cart.Interfaces;
using Catalog.Application.Product.Interfaces;
using Moq;
using Orders.Application.Order.Commands.PlaceOrder;
using Orders.Application.Order.Interfaces;
using RetailHub.SharedKernel.Application.Common.Abstractions;

namespace RetailHub.Services.Tests.Order.PlaceOrderCommand;

public sealed class PlaceOrderCommandHandlerBuilder
{
    private Mock<ICartRepository> _cartRepository = new();
    private Mock<IOrderRepository> _orderRepository = new();
    private Mock<IProductReadRepository> _productReadRepository = new();
    private Mock<IUserIdentityLookup> _userIdentityLookup = new();

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

    public PlaceOrderCommandHandlerBuilder WithProductReadRepository(Mock<IProductReadRepository> productReadRepository)
    {
        _productReadRepository = productReadRepository;
        return this;
    }

    public PlaceOrderCommandHandlerBuilder WithUserIdentityLookup(Mock<IUserIdentityLookup> userIdentityLookup)
    {
        _userIdentityLookup = userIdentityLookup;
        return this;
    }

    public PlaceOrderCommandHandler Build() =>
        new(
            _cartRepository.Object,
            _orderRepository.Object,
            _productReadRepository.Object,
            _userIdentityLookup.Object);
}
