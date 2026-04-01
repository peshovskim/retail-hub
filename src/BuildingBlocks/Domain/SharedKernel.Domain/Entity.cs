namespace RetailHub.SharedKernel.Domain;

/// <summary>
/// Base for domain objects with identity and audit/soft-delete fields. Domain events live on <see cref="AggregateRoot"/>.
/// </summary>
public abstract class Entity
{
    public Guid Id { get; protected set; }

    public DateTime CreatedOn { get; protected set; }

    public DateTime? DeletedOn { get; protected set; }

    protected Entity()
    {
    }
}
