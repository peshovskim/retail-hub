using CategoryEntity = Catalog.Domain.Category.Domain.Category;

namespace Catalog.Infrastructure.Persistence.Write.Category.Factories;

public static class CategoryFactory
{
    public static CategoryEntity Create(DateTime createdOn, string name, string slug, int? parentId) =>
        CategoryEntity.Create(createdOn, name, slug, parentId);
}
