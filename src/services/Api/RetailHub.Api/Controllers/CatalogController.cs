using Catalog.Application.Category.Queries.GetCategories;
using Catalog.Application.Category.Queries.GetCategoryMenu;
using Catalog.Application.Category.Responses;
using Catalog.Application.Product.Queries.GetProductById;
using Catalog.Application.Product.Queries.GetProducts;
using Catalog.Application.Product.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RetailHub.SharedKernel.Domain;

namespace RetailHub.Api.Controllers;

[ApiController]
[Route("api/catalog")]
public sealed class CatalogController : ExtendedApiController
{
    private readonly IMediator _mediator;

    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("categories")]
    [ProducesResponseType(typeof(IReadOnlyList<CategoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        Result<IReadOnlyList<CategoryResponse>> result = await _mediator
            .Send(new GetCategoriesQuery(), cancellationToken);

        return OkOrError(result);
    }

    [HttpGet("categories/menu")]
    [ProducesResponseType(typeof(IReadOnlyList<CategoryMenuNodeResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategoryMenu(CancellationToken cancellationToken)
    {
        Result<IReadOnlyList<CategoryMenuNodeResponse>> result = await _mediator
            .Send(new GetCategoryMenuQuery(), cancellationToken);

        return OkOrError(result);
    }

    [HttpGet("products")]
    [ProducesResponseType(typeof(ProductListResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsQuery query, CancellationToken cancellationToken)
    {
        Result<ProductListResult> result = await _mediator
            .Send(query, cancellationToken);

        return OkOrError(result);
    }

    [HttpGet("products/{id:guid}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        Result<ProductResponse> result = await _mediator
            .Send(new GetProductByIdQuery(id), cancellationToken);

        return OkOrError(result);
    }
}
