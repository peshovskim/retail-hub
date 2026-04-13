using Cart.Application.Cart.Interfaces;
using CartEntity = Cart.Domain.Cart.Domain.Cart;
using Catalog.Application.Product.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderAggregate = Orders.Domain.Order.Domain.Order;
using Orders.Application.Order.Interfaces;
using Orders.Application.Order.Responses;
using Orders.Domain.Order.ValueObjects;
using RetailHub.SharedKernel.Application.Common.Abstractions;
using RetailHub.SharedKernel.Application.Common.Cqrs;
using RetailHub.SharedKernel.Domain;

namespace Orders.Application.Order.Commands.PlaceOrder;

public sealed record PlaceOrderCommand(Guid CartId, Guid? UserId) : ICommand<OrderResponse>;

public sealed class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Result<OrderResponse>>
{
    public const string NewOrderStatus = "Pending";

    private readonly ICartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IUserIdentityLookup _userIdentityLookup;
    private readonly ILogger<PlaceOrderCommandHandler> _logger;

    public PlaceOrderCommandHandler(
        ICartRepository cartRepository,
        IOrderRepository orderRepository,
        IProductReadRepository productReadRepository,
        IUserIdentityLookup userIdentityLookup,
        ILogger<PlaceOrderCommandHandler> logger)
    {
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
        _productReadRepository = productReadRepository;
        _userIdentityLookup = userIdentityLookup;
        _logger = logger;
    }

    public async Task<Result<OrderResponse>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        CartEntity? cart = await _cartRepository
            .GetByIdWithItemsAsync(request.CartId, cancellationToken);

        if (cart is null)
        {
            return Result<OrderResponse>.NotFound(ResultCodes.NotFound, "Cart not found.");
        }

        DateTime utcNow = DateTime.UtcNow;

        int? userId = null;
        if (request.UserId is { } userUid)
        {
            userId = await _userIdentityLookup
                .GetUserIdByUidAsync(userUid, cancellationToken);
        }

        List<int> activeProductIds = cart.Items.Where(i => i.IsActive).Select(i => i.ProductId).Distinct().ToList();
        IReadOnlyDictionary<int, Guid> productUidById = await _productReadRepository
            .GetProductUidsByIdsAsync(activeProductIds, cancellationToken);

        if (productUidById.Count != activeProductIds.Count)
        {
            return Result<OrderResponse>.Invalid(
                ResultCodes.Validation,
                "One or more cart products could not be resolved.");
        }

        CartPlacementSnapshot placement = CartPlacementSnapshotMapper.FromCart(cart);
        Result<OrderAggregate> orderResult = OrderAggregate.PlaceFromCartPlacement(
            placement,
            userId,
            request.UserId,
            productUidById,
            NewOrderStatus,
            utcNow);

        if (orderResult.IsFailure)
        {
            return Result.FromError<OrderResponse>(orderResult);
        }

        OrderAggregate order = orderResult.Value!;

        await _orderRepository.AddAsync(order, cancellationToken);
        await _orderRepository.SaveChangesAsync(cancellationToken);

        cart.ClearAllActiveItems(utcNow);
        await _cartRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Order {OrderUid} placed from cart {CartUid} for {TotalAmount} with {LineCount} line(s)",
            order.Uid,
            order.CartUid,
            order.TotalAmount,
            order.Lines.Count);

        return Result<OrderResponse>.Success(ToResponse(order));
    }

    private static OrderResponse ToResponse(OrderAggregate order)
    {
        List<OrderLineResponse> lines = order.Lines
            .OrderBy(l => l.ProductId)
            .Select(l => new OrderLineResponse(l.ProductUid, l.Quantity, l.UnitPrice, l.LineTotal))
            .ToList();

        return new OrderResponse(
            order.Uid,
            order.UserUid,
            order.CartUid,
            order.Status,
            order.TotalAmount,
            lines);
    }
}
