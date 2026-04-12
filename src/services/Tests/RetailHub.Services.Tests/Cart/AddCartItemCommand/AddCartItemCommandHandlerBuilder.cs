using Cart.Application.Cart.Commands.AddCartItem;
using Cart.Application.Cart.Interfaces;
using Catalog.Application.Product.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace RetailHub.Services.Tests.Cart.AddCartItemCommand;

public sealed class AddCartItemCommandHandlerBuilder
{
    private Mock<ICartRepository> _cartRepository = new();
    private Mock<IProductReadRepository> _productReadRepository = new();

    public AddCartItemCommandHandlerBuilder WithCartRepository(Mock<ICartRepository> cartRepository)
    {
        _cartRepository = cartRepository;
        return this;
    }

    public AddCartItemCommandHandlerBuilder WithProductReadRepository(Mock<IProductReadRepository> productReadRepository)
    {
        _productReadRepository = productReadRepository;
        return this;
    }

    public AddCartItemCommandHandler Build() =>
        new(
            _cartRepository.Object,
            _productReadRepository.Object,
            NullLogger<AddCartItemCommandHandler>.Instance);
}
