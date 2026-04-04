using FluentValidation;

namespace Cart.Application.Cart.Commands.UpdateCartItemQuantity;

public sealed class UpdateCartItemQuantityCommandValidator : AbstractValidator<UpdateCartItemQuantityCommand>
{
    public const int MaxQuantity = 99;

    public UpdateCartItemQuantityCommandValidator()
    {
        RuleFor(x => x.CartId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).InclusiveBetween(0, MaxQuantity);
    }
}
