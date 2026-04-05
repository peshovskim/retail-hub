using Cart.Application.Cart.Interfaces;
using FluentAssertions;
using NUnit.Framework;
using Moq;
using RetailHub.SharedKernel.Domain;
using CartEntity = Cart.Domain.Cart.Domain.Cart;
using SessionCommand = Cart.Application.Cart.Commands.CreateOrGetCartSession.CreateOrGetCartSessionCommand;

namespace RetailHub.Services.Tests.Cart.CreateOrGetCartSessionCommand;

[TestFixture(Category = nameof(CreateOrGetCartSessionCommandTests))]
public sealed class CreateOrGetCartSessionCommandTests
{
    [Test]
    public async Task CreateOrGetCartSessionCommand_KeyTooLong_ReturnsInvalid()
    {
        var key = new string('x', 129);
        var command = new SessionCommand(key);

        var cartRepo = new Mock<ICartRepository>();

        var handler = new CreateOrGetCartSessionCommandHandlerBuilder()
            .WithCartRepository(cartRepo)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be(ResultCodes.Validation);
        cartRepo.Verify(x => x.GetByAnonymousKeyAsNoTrackingAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    public async Task CreateOrGetCartSessionCommand_ExistingKey_ReturnsExistingCart()
    {
        var cartId = Guid.NewGuid();
        var key = "  stable-key  ";
        var existing = CartTestsHelper.CreateCart(id: cartId, anonymousKey: key.Trim());
        var command = new SessionCommand(key);

        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByAnonymousKeyAsNoTrackingAsync(key.Trim(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        var handler = new CreateOrGetCartSessionCommandHandlerBuilder()
            .WithCartRepository(cartRepo)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.CartId.Should().Be(cartId);
        result.Value.AnonymousKey.Should().Be(key.Trim());
        cartRepo.Verify(x => x.AddAsync(It.IsAny<CartEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task CreateOrGetCartSessionCommand_NewKey_PersistsCart()
    {
        var key = "brand-new-session";
        var command = new SessionCommand(key);

        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByAnonymousKeyAsNoTrackingAsync(key, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CartEntity?)null);

        var handler = new CreateOrGetCartSessionCommandHandlerBuilder()
            .WithCartRepository(cartRepo)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.AnonymousKey.Should().Be(key);
        cartRepo.Verify(x => x.AddAsync(It.Is<CartEntity>(c => c.AnonymousKey == key), It.IsAny<CancellationToken>()),
            Times.Once);
        cartRepo.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task CreateOrGetCartSessionCommand_NullKeyGeneratesNewKeyAndCreatesCart()
    {
        var command = new SessionCommand(null);

        string? capturedKey = null;
        var cartRepo = new Mock<ICartRepository>();
        cartRepo.Setup(x => x.GetByAnonymousKeyAsNoTrackingAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CartEntity?)null)
            .Callback<string, CancellationToken>((k, _) => capturedKey = k);

        var handler = new CreateOrGetCartSessionCommandHandlerBuilder()
            .WithCartRepository(cartRepo)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        capturedKey.Should().NotBeNullOrWhiteSpace();
        result.Value!.AnonymousKey.Should().Be(capturedKey);
        cartRepo.Verify(x => x.AddAsync(It.IsAny<CartEntity>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
