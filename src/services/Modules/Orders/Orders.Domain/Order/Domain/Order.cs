using RetailHub.SharedKernel.Domain;

namespace Orders.Domain.Order.Domain;

public sealed partial class Order : AggregateRoot
{
    private Order()
    {
    }

    public int? UserId { get; private set; }

    public Guid? UserUid { get; private set; }

    public string Status { get; private set; } = null!;

    public int? CartId { get; private set; }

    public Guid? CartUid { get; private set; }

    public decimal TotalAmount { get; private set; }

    public ICollection<OrderLine> Lines { get; } = new List<OrderLine>();
}
