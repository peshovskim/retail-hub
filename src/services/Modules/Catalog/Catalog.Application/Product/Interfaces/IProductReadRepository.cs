using Catalog.Application.Product.Responses;

namespace Catalog.Application.Product.Interfaces;

public interface IProductReadRepository
{
    Task<IReadOnlyList<ProductResponse>> GetAllActiveProductsAsync(CancellationToken cancellationToken = default);

    Task<ProductResponse?> GetActiveProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
