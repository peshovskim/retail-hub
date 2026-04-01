using Catalog.Contracts.Category.Responses;

namespace Catalog.Application.Category.Interfaces;

public interface ICategoryReadRepository
{
    Task<IReadOnlyList<CategoryResponse>> GetRootCategoriesAsync(CancellationToken cancellationToken = default);
}
