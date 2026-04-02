using Catalog.Application.Category.Interfaces;
using Catalog.Application.Category.Responses;
using Catalog.Infrastructure.Read.Category.Factories;
using CategoryEntity = Catalog.Domain.Category.Domain.Category;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Read.Category.Queries;

internal sealed class CategoryQueries(CatalogReadDbContext db, CategoryReadFactory readFactory)
    : ICategoryReadRepository
{
    public async Task<IReadOnlyList<CategoryResponse>> GetRootCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await db.Set<CategoryEntity>()
            .Where(c => c.ParentId == null && c.DeletedOn == null)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return rows.Select(readFactory.ToResponse).ToList();
    }
}

