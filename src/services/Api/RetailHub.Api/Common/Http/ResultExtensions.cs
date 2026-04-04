using Microsoft.AspNetCore.Mvc;
using RetailHub.SharedKernel.Domain;

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

        return error.Type switch
        {
            ResultType.Invalid => new BadRequestObjectResult(new { code = error.Code, message = error.Message }),
            ResultType.NotFound => new NotFoundObjectResult(new { code = error.Code, message = error.Message }),
            ResultType.Conflicted => new ConflictObjectResult(new { code = error.Code, message = error.Message }),
            ResultType.Forbidden => new ObjectResult(new { code = error.Code, message = error.Message })
            {
                StatusCode = StatusCodes.Status403Forbidden,
            },
            ResultType.Unauthorized => new ObjectResult(new { code = error.Code, message = error.Message })
            {
                StatusCode = StatusCodes.Status401Unauthorized,
            },
            ResultType.InternalError or ResultType.Ok => new ObjectResult(new { code = error.Code, message = error.Message })
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            },
            _ => new ObjectResult(new { code = error.Code, message = error.Message })
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            },
        };
    }
}
