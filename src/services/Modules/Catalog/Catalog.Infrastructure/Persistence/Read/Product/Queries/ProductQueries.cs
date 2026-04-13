using Catalog.Application.Product.Interfaces;
using Catalog.Application.Product.Queries.GetProducts;
using Catalog.Application.Product.Responses;
using Catalog.Infrastructure.Persistence.Read.Product.Factories;
using Microsoft.EntityFrameworkCore;
using ProductEntity = Catalog.Domain.Product.Domain.Product;
using ProductImageEntity = Catalog.Domain.Product.Domain.ProductImage;

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
        IQueryable<ProductEntity> query = BaseActiveProducts();

        if (criteria.CategoryIds is { Count: > 0 } categoryIds)
        {
            query = query.Where(p =>
                categoryIds.Contains(p.CategoryId)
                && _dbContext.Categories.Any(c => c.Id == p.CategoryId && c.DeletedOn == null));
        }

        if (criteria.PriceMin is { } priceMin)
        {
            query = query.Where(p => p.Price >= priceMin);
        }

        if (criteria.PriceMax is { } priceMax)
        {
            query = query.Where(p => p.Price <= priceMax);
        }

        string? search = criteria.Search?.Trim();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Name.Contains(search) || p.Sku.Contains(search));
        }

        query = ApplySort(query, criteria.Sort);

        int totalCount = await query.CountAsync(cancellationToken);

        if (criteria.Page is { } page && criteria.PageSize is { } pageSize)
        {
            int skip = (page - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);
        }

        List<ProductEntity> rows = await (
                from p in query
                join c in _dbContext.Categories on p.CategoryId equals c.Id
                where c.DeletedOn == null
                select p)
            .ToListAsync(cancellationToken);

        Dictionary<int, IReadOnlyList<ProductImageResponse>> imagesByProductId = await LoadImagesByProductIdAsync(
                rows.ConvertAll(p => p.Id),
                cancellationToken);

        List<ProductResponse> items = rows.ConvertAll(p =>
            _readFactory.ToResponse(
                p,
                images: imagesByProductId.GetValueOrDefault(p.Id)));
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

    public async Task<ProductResponse?> GetActiveProductByUidAsync(
        Guid uid,
        CancellationToken cancellationToken = default)
    {
        var row = await (
                from p in _dbContext.Products
                join c in _dbContext.Categories on p.CategoryId equals c.Id
                where p.Uid == uid && p.DeletedOn == null && c.DeletedOn == null
                select new { Product = p, c.Name })
            .FirstOrDefaultAsync(cancellationToken);

        if (row is null)
        {
            return null;
        }

        IReadOnlyList<ProductImageResponse> images = await LoadImagesForProductAsync(row.Product.Id, cancellationToken);
        return _readFactory.ToResponse(row.Product, row.Name, images);
    }

    public async Task<ProductResponse?> GetActiveProductByInternalIdAsync(
        int productId,
        CancellationToken cancellationToken = default)
    {
        var row = await (
                from p in _dbContext.Products
                join c in _dbContext.Categories on p.CategoryId equals c.Id
                where p.Id == productId && p.DeletedOn == null && c.DeletedOn == null
                select new { Product = p, c.Name })
            .FirstOrDefaultAsync(cancellationToken);

        if (row is null)
        {
            return null;
        }

        IReadOnlyList<ProductImageResponse> images = await LoadImagesForProductAsync(row.Product.Id, cancellationToken);
        return _readFactory.ToResponse(row.Product, row.Name, images);
    }

    public async Task<IReadOnlyDictionary<int, Guid>> GetProductUidsByIdsAsync(
        IEnumerable<int> productIds,
        CancellationToken cancellationToken = default)
    {
        int[] ids = productIds.Distinct().ToArray();
        if (ids.Length == 0)
        {
            return new Dictionary<int, Guid>();
        }

        var rows = await _dbContext.Products
            .AsNoTracking()
            .Where(p => ids.Contains(p.Id) && p.DeletedOn == null)
            .Select(p => new { p.Id, p.Uid })
            .ToListAsync(cancellationToken);

        return rows.ToDictionary(r => r.Id, r => r.Uid);
    }

    private async Task<IReadOnlyList<ProductImageResponse>> LoadImagesForProductAsync(
        int productId,
        CancellationToken cancellationToken)
    {
        List<ProductImageResponse> imageRows = await _dbContext.ProductImages
            .AsNoTracking()
            .Where(pi => pi.ProductId == productId && pi.DeletedOn == null)
            .OrderBy(pi => pi.SortOrder)
            .ThenBy(pi => pi.Uid)
            .Select(pi => new ProductImageResponse(pi.Uid, pi.SortOrder, pi.ImageUrl))
            .ToListAsync(cancellationToken);

        return imageRows;
    }

    private async Task<Dictionary<int, IReadOnlyList<ProductImageResponse>>> LoadImagesByProductIdAsync(
        IReadOnlyList<int> productIds,
        CancellationToken cancellationToken)
    {
        if (productIds.Count == 0)
        {
            return new Dictionary<int, IReadOnlyList<ProductImageResponse>>();
        }

        List<ProductImageEntity> imageEntities = await _dbContext.ProductImages
            .AsNoTracking()
            .Where(pi => productIds.Contains(pi.ProductId) && pi.DeletedOn == null)
            .ToListAsync(cancellationToken);

        return imageEntities
            .GroupBy(pi => pi.ProductId)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyList<ProductImageResponse>)g
                    .OrderBy(pi => pi.SortOrder)
                    .ThenBy(pi => pi.Uid)
                    .Select(pi => new ProductImageResponse(pi.Uid, pi.SortOrder, pi.ImageUrl))
                    .ToList());
    }
}
