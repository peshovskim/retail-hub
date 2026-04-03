using FluentValidation;

namespace Catalog.Application.Product.Queries.GetProducts;

public sealed class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public const int MaxSearchLength = 200;
    public const int MaxPageSize = 100;

    public GetProductsQueryValidator()
    {
        RuleFor(x => x.Search)
            .Must(static s => string.IsNullOrWhiteSpace(s) || s.Trim().Length <= MaxSearchLength)
            .WithMessage($"Search must be at most {MaxSearchLength} characters after trimming.");

        RuleFor(x => x.PriceMin)
            .GreaterThanOrEqualTo(0)
            .When(x => x.PriceMin.HasValue);

        RuleFor(x => x.PriceMax)
            .GreaterThanOrEqualTo(0)
            .When(x => x.PriceMax.HasValue);

        RuleFor(x => x)
            .Must(static x => !x.PriceMin.HasValue || !x.PriceMax.HasValue || x.PriceMin <= x.PriceMax)
            .WithMessage("PriceMin must be less than or equal to PriceMax.");

        RuleFor(x => x.CategoryIds)
            .Must(static ids => ids is null || ids.All(id => id != Guid.Empty))
            .WithMessage("Category id must not be empty.");

        RuleFor(x => x)
            .Must(static x => x.Page.HasValue == x.PageSize.HasValue)
            .WithMessage("Page and PageSize must both be set or both omitted.");

        When(x => x.Page.HasValue, () =>
        {
            RuleFor(x => x.Page!.Value).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize!.Value).InclusiveBetween(1, MaxPageSize);
        });
    }
}
