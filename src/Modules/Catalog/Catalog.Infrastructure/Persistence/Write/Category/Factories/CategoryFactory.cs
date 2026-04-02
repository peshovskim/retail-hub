using CategoryEntity = Catalog.Domain.Category.Domain.Category;

namespace Catalog.Infrastructure.Persistence.Write.Category.Factories;

public static class CategoryFactory
{
    public static CategoryEntity Create(Guid id, DateTime createdOn, string name, string slug, Guid? parentId) =>
        CategoryEntity.Create(id, createdOn, name, slug, parentId);
}
