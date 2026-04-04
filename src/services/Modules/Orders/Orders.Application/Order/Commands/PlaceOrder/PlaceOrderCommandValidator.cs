using FluentValidation;

namespace Orders.Application.Order.Commands.PlaceOrder;

public sealed class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {
        RuleFor(x => x.CartId).NotEmpty();
    }
}
