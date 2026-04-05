using Cart.Application.Cart.Commands.CreateOrGetCartSession;
using Cart.Application.Cart.Interfaces;
using Moq;

namespace RetailHub.Services.Tests.Cart.CreateOrGetCartSessionCommand;

public sealed class CreateOrGetCartSessionCommandHandlerBuilder
{
    private Mock<ICartRepository> _cartRepository = new();

    public CreateOrGetCartSessionCommandHandlerBuilder WithCartRepository(Mock<ICartRepository> cartRepository)
    {
        _cartRepository = cartRepository;
        return this;
    }

    public CreateOrGetCartSessionCommandHandler Build() =>
        new(_cartRepository.Object);
}
