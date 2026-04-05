using Cart.Application.Cart.Commands.UpdateCartItemQuantity;
using Cart.Application.Cart.Interfaces;
using Catalog.Application.Product.Interfaces;
using Moq;

namespace RetailHub.Services.Tests.Cart.UpdateCartItemQuantityCommand;

public sealed class UpdateCartItemQuantityCommandHandlerBuilder
{
    private Mock<ICartRepository> _cartRepository = new();
    private Mock<IProductReadRepository> _productReadRepository = new();

    public UpdateCartItemQuantityCommandHandlerBuilder WithCartRepository(Mock<ICartRepository> cartRepository)
    {
        _cartRepository = cartRepository;
        return this;
    }

    public UpdateCartItemQuantityCommandHandlerBuilder WithProductReadRepository(Mock<IProductReadRepository> productReadRepository)
    {
        _productReadRepository = productReadRepository;
        return this;
    }

    public UpdateCartItemQuantityCommandHandler Build() =>
        new(_cartRepository.Object, _productReadRepository.Object);
}
