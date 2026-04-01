using Microsoft.AspNetCore.Mvc;
using RetailHub.SharedKernel.Application.Common.Results;

namespace RetailHub.Api.Common.Http;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result.Value);
        }

        var error = result.Error!;
        return error.Code switch
        {
            ErrorCodes.Validation => new BadRequestObjectResult(new { code = error.Code, message = error.Message }),
            ErrorCodes.NotFound => new NotFoundObjectResult(new { code = error.Code, message = error.Message }),
            ErrorCodes.Conflict => new ConflictObjectResult(new { code = error.Code, message = error.Message }),
            _ => new ObjectResult(new { code = error.Code, message = error.Message })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }
}
