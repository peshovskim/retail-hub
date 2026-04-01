using Catalog.Application.Category.Interfaces;
using CategoryEntity = Catalog.Domain.Category.Domain.Category;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Write.Category.Repositories;

internal sealed class CategoryRepository(CatalogDbContext db) : ICategoryRepository
{
    public async Task AddAsync(CategoryEntity category, CancellationToken cancellationToken = default)
    {
        await db.Set<CategoryEntity>().AddAsync(category, cancellationToken).ConfigureAwait(false);
    }
}
