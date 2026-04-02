using Catalog.Application.Category.Responses;
using CategoryEntity = Catalog.Domain.Category.Domain.Category;

namespace Catalog.Infrastructure.Persistence.Read.Category.Factories;

public sealed class CategoryReadFactory
{
    public CategoryResponse ToResponse(CategoryEntity category) =>
        new(category.Id, category.Name, category.Slug);
}

