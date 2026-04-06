using RetailHub.SharedKernel.Domain;

namespace Orders.Domain.Order.Domain;

public sealed partial class OrderLine : Entity
{
    private OrderLine()
    {
    }

    public int OrderId { get; private set; }

    public int ProductId { get; private set; }

    public Guid ProductUid { get; private set; }

    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public decimal LineTotal { get; private set; }

    public Order Order { get; set; } = null!;
}
