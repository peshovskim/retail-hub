using Catalog.Application.Category.Interfaces;
using Catalog.Application.Category.Responses;
using Catalog.Infrastructure.Persistence.Read.Category.Factories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence.Read.Category.Queries;

internal sealed class CategoryQueries : ICategoryReadRepository
{
    private readonly CatalogReadDbContext _dbContext;
    private readonly CategoryReadFactory _readFactory;

    public CategoryQueries(CatalogReadDbContext dbContext, CategoryReadFactory readFactory)
    {
        _dbContext = dbContext;
        _readFactory = readFactory;
    }

    public async Task<IReadOnlyList<CategoryResponse>> GetRootCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await _dbContext.Categories
            .Where(c => c.ParentId == null && c.DeletedOn == null)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return rows.Select(_readFactory.ToResponse).ToList();
    }

    public async Task<IReadOnlyList<CategoryMenuSourceRow>> GetAllActiveCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await (
                from c in _dbContext.Categories.Where(c => c.DeletedOn == null)
                join p in _dbContext.Categories on c.ParentId equals (int?)p.Id into parents
                from p in parents.DefaultIfEmpty()
                orderby c.Name
                select new CategoryMenuSourceRow(c.Id, c.Name, c.Slug, p == null ? null : (int?)p.Id))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return rows;
    }
}
