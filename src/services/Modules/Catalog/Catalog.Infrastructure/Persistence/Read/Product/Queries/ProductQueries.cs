using Catalog.Application.Product.Interfaces;
using Catalog.Application.Product.Queries.GetProducts;
using Catalog.Application.Product.Responses;
using Catalog.Infrastructure.Persistence.Read.Product.Factories;
using Microsoft.EntityFrameworkCore;
using ProductEntity = Catalog.Domain.Product.Domain.Product;

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

    public async Task<ProductListResult> ListActiveProductsAsync(
        GetProductsQuery criteria,
        CancellationToken cancellationToken = default)
    {
        var query = BaseActiveProducts();

        if (criteria.CategoryIds is { Count: > 0 } categoryIds)
        {
            query = query.Where(p => categoryIds.Contains(p.CategoryId));
        }

        if (criteria.PriceMin is { } priceMin)
        {
            query = query.Where(p => p.Price >= priceMin);
        }

        if (criteria.PriceMax is { } priceMax)
        {
            query = query.Where(p => p.Price <= priceMax);
        }

        var search = criteria.Search?.Trim();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Name.Contains(search) || p.Sku.Contains(search));
        }

        query = ApplySort(query, criteria.Sort);

        var totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        if (criteria.Page is { } page && criteria.PageSize is { } pageSize)
        {
            var skip = (page - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
        }

        var rows = await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        var items = rows.ConvertAll(p => _readFactory.ToResponse(p));
        return new ProductListResult(items, totalCount);
    }

    private IQueryable<ProductEntity> BaseActiveProducts() =>
        _dbContext.Products.AsNoTracking().Where(p => p.DeletedOn == null);

    private static IQueryable<ProductEntity> ApplySort(IQueryable<ProductEntity> query, ProductListSort sort) =>
        sort switch
        {
            ProductListSort.NameAsc => query.OrderBy(p => p.Name),
            ProductListSort.NameDesc => query.OrderByDescending(p => p.Name),
            ProductListSort.PriceAsc => query.OrderBy(p => p.Price),
            ProductListSort.PriceDesc => query.OrderByDescending(p => p.Price),
            _ => query.OrderBy(p => p.Name),
        };

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
