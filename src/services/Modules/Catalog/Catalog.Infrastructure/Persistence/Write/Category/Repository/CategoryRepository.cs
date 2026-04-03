using Catalog.Application.Category.Interfaces;
using CategoryEntity = Catalog.Domain.Category.Domain.Category;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence.Write.Category.Repository;

internal sealed class CategoryRepository : ICategoryRepository
{
    private readonly CatalogWriteDbContext _db;

    public CategoryRepository(CatalogWriteDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(CategoryEntity category, CancellationToken cancellationToken = default)
    {
        await _db.Set<CategoryEntity>().AddAsync(category, cancellationToken).ConfigureAwait(false);
    }
}
