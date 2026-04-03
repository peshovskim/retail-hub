using Catalog.Application.Category.Interfaces;
using CategoryEntity = Catalog.Domain.Category.Domain.Category;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence.Write.Category.Repository;

internal sealed class CategoryRepository : ICategoryRepository
{
    private readonly CatalogWriteDbContext _dbContext;

    public CategoryRepository(CatalogWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(CategoryEntity category, CancellationToken cancellationToken = default)
    {
        await _dbContext.Categories.AddAsync(category, cancellationToken).ConfigureAwait(false);
    }
}
