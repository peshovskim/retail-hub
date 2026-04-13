using Cart.Application.Cart.Interfaces;
using Cart.Application.Cart.Responses;
using CartEntity = Cart.Domain.Cart.Domain.Cart;
using MediatR;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Domain;

namespace Cart.Application.Cart.Commands.CreateOrGetCartSession;

/// <param name="ClientAnonymousKey">Optional key from the client (e.g. header). When null or empty, a new cart is created.</param>
public sealed record CreateOrGetCartSessionCommand(string? ClientAnonymousKey) : ICommand<CartSessionResponse>;

public sealed class CreateOrGetCartSessionCommandHandler : IRequestHandler<CreateOrGetCartSessionCommand, Result<CartSessionResponse>>
{
    private readonly ICartRepository _cartRepository;

    public CreateOrGetCartSessionCommandHandler(ICartRepository cartRepository) => _cartRepository = cartRepository;

    public async Task<Result<CartSessionResponse>> Handle(
        CreateOrGetCartSessionCommand request,
        CancellationToken cancellationToken)
    {
        string key = string.IsNullOrWhiteSpace(request.ClientAnonymousKey)
            ? Guid.NewGuid().ToString("D")
            : request.ClientAnonymousKey.Trim();

        if (key.Length > 128)
        {
            return Result<CartSessionResponse>.Invalid(
                ResultCodes.Validation,
                "Anonymous session key must be at most 128 characters.");
        }

        CartEntity? existing = await _cartRepository
            .GetByAnonymousKeyAsNoTrackingAsync(key, cancellationToken);

        if (existing is not null)
        {
            return Result<CartSessionResponse>.Success(new CartSessionResponse(existing.Uid, key));
        }

        CartEntity cart = CartEntity.Create(DateTime.UtcNow, userId: null, anonymousKey: key);

        await _cartRepository.AddAsync(cart, cancellationToken);
        await _cartRepository.SaveChangesAsync(cancellationToken);

        return Result<CartSessionResponse>.Success(new CartSessionResponse(cart.Uid, key));
    }
}
