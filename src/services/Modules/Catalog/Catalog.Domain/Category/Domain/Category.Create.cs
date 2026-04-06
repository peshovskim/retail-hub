using Catalog.Domain.Category.DomainEvents;

namespace Catalog.Domain.Category.Domain;

public sealed partial class Category
{
    private Category()
    {
    }

    public static Category Create(DateTime createdOn, string name, string slug, int? parentId)
    {
        var category = new Category
        {
            Uid = Guid.NewGuid(),
            CreatedOn = createdOn,
            Name = name,
            Slug = slug,
            ParentId = parentId,
            DeletedOn = null
        };
        category.AddDomainEvent(new CategoryCreatedDomainEvent(category.Uid, name, slug, createdOn));
        return category;
    }
}
