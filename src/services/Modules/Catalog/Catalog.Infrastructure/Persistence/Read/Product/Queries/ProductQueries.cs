using Catalog.Application.Product.Interfaces;
using Catalog.Application.Product.Responses;
using Catalog.Infrastructure.Persistence.Read.Product.Factories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence.Read.Product.Queries;

internal sealed class ProductQueries : IProductReadRepository
{
    private readonly CatalogReadDbContext _dbContext;
    private readonly ProductReadFactory _readFactory;

    public ProductQueries(CatalogReadDbContext dbContext, ProductReadFactory readFactory)
    {
        _dbContext = dbContext;
        _readFactory = readFactory;
    }

    public async Task<IReadOnlyList<ProductResponse>> GetAllActiveProductsAsync(
        CancellationToken cancellationToken = default)
    {
        var rows = await _dbContext.Products
            .Where(p => p.DeletedOn == null)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return rows.Select(p => _readFactory.ToResponse(p)).ToList();
    }

    public async Task<ProductResponse?> GetActiveProductByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var row = await (
                from p in _dbContext.Products
                join c in _dbContext.Categories on p.CategoryId equals c.Id
                where p.Id == id && p.DeletedOn == null && c.DeletedOn == null
                select new { Product = p, c.Name })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return row is null ? null : _readFactory.ToResponse(row.Product, row.Name);
    }
}
