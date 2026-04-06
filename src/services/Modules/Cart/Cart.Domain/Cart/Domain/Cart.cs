using RetailHub.SharedKernel.Domain;

namespace Cart.Domain.Cart.Domain;

public sealed partial class Cart : AggregateRoot
{
    private Cart()
    {
    }

    public int? UserId { get; private set; }

    public string? AnonymousKey { get; private set; }

    public ICollection<CartItem> Items { get; } = new List<CartItem>();
}
