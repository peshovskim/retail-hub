using Catalog.Domain.Category.DomainEvents;

namespace Catalog.Domain.Category.Domain;

public sealed partial class Category
{
    private Category()
    {
    }

    public static Category Create(Guid id, DateTime createdOn, string name, string slug, Guid? parentId)
    {
        var category = new Category
        {
            Id = id,
            CreatedOn = createdOn,
            Name = name,
            Slug = slug,
            ParentId = parentId,
            DeletedOn = null
        };
        category.AddDomainEvent(new CategoryCreatedDomainEvent(id, name, slug, parentId, createdOn));
        return category;
    }
}
