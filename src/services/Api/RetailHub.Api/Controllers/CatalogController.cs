using Catalog.Application.Category.Queries.GetCategories;
using Catalog.Application.Category.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RetailHub.Api.Common.Http;

namespace RetailHub.Api.Controllers;

[ApiController]
[Route("api/catalog")]
public sealed class CatalogController(IMediator mediator) : ControllerBase
{
    [HttpGet("categories")]
    [ProducesResponseType(typeof(IReadOnlyList<CategoryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCategoriesQuery(), cancellationToken).ConfigureAwait(false);
        return result.ToActionResult();
    }
}

