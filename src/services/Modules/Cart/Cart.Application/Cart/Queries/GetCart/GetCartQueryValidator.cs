using FluentValidation;

namespace Cart.Application.Cart.Queries.GetCart;

public sealed class GetCartQueryValidator : AbstractValidator<GetCartQuery>
{
    public GetCartQueryValidator()
    {
        RuleFor(x => x.CartId).NotEmpty();
    }
}
