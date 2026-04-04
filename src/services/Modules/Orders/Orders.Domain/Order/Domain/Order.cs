using RetailHub.SharedKernel.Domain;

namespace Orders.Domain.Order.Domain;

public sealed partial class Order : AggregateRoot
{
    private Order()
    {
    }

    public Guid? UserId { get; private set; }

    public string Status { get; private set; } = null!;

    public Guid? CartId { get; private set; }

    public decimal TotalAmount { get; private set; }

    public ICollection<OrderLine> Lines { get; } = new List<OrderLine>();
}
