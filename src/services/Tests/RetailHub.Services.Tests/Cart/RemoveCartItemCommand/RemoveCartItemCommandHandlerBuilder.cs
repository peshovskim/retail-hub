using Cart.Application.Cart.Commands.RemoveCartItem;
using Cart.Application.Cart.Interfaces;
using Catalog.Application.Product.Interfaces;
using Moq;

namespace RetailHub.Services.Tests.Cart.RemoveCartItemCommand;

public sealed class RemoveCartItemCommandHandlerBuilder
{
    private Mock<ICartRepository> _cartRepository = new();
    private Mock<IProductReadRepository> _productReadRepository = new();

    public RemoveCartItemCommandHandlerBuilder WithCartRepository(Mock<ICartRepository> cartRepository)
    {
        _cartRepository = cartRepository;
        return this;
    }

    public RemoveCartItemCommandHandlerBuilder WithProductReadRepository(Mock<IProductReadRepository> productReadRepository)
    {
        _productReadRepository = productReadRepository;
        return this;
    }

    public RemoveCartItemCommandHandler Build() =>
        new(_cartRepository.Object, _productReadRepository.Object);
}
