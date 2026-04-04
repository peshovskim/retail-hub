using RetailHub.SharedKernel.Domain;

namespace Cart.Domain.Cart.Domain;

public sealed partial class CartItem : Entity
{
    public Guid CartId { get; private set; }

    public Guid ProductId { get; private set; }

    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public DateTime? UpdatedOn { get; private set; }

    public Cart Cart { get; set; } = null!;

    public bool IsActive => DeletedOn is null;
}
