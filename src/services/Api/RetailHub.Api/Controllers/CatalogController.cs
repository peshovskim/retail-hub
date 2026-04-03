using Catalog.Application.Category.Queries.GetCategories;
using Catalog.Application.Category.Queries.GetCategoryMenu;
using Catalog.Application.Category.Responses;
using Catalog.Application.Product.Queries.GetProducts;
using Catalog.Application.Product.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RetailHub.Api.Common.Http;

namespace RetailHub.Api.Controllers;

[ApiController]
[Route("api/catalog")]
public sealed class CatalogController : ControllerBase
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
        var result = await _mediator.Send(new GetCategoriesQuery(), cancellationToken).ConfigureAwait(false);
        return result.ToActionResult();
    }

    [HttpGet("categories/menu")]
    [ProducesResponseType(typeof(IReadOnlyList<CategoryMenuNodeResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategoryMenu(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCategoryMenuQuery(), cancellationToken).ConfigureAwait(false);
        return result.ToActionResult();
    }

    [HttpGet("products")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProductsQuery(), cancellationToken).ConfigureAwait(false);
        return result.ToActionResult();
    }
}
