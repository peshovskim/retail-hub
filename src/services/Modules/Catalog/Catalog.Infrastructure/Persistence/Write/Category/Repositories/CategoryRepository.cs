using Catalog.Application.Category.Interfaces;
using CategoryEntity = Catalog.Domain.Category.Domain.Category;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence.Write.Category.Repositories;

internal sealed class CategoryRepository(CatalogWriteDbContext db) : ICategoryRepository
{
    public async Task AddAsync(CategoryEntity category, CancellationToken cancellationToken = default)
    {
        await db.Set<CategoryEntity>().AddAsync(category, cancellationToken).ConfigureAwait(false);
    }
}
