using RetailHub.SharedKernel.Application.Common.Results;
using RetailHub.SharedKernel.Domain;

namespace Cart.Application.Cart;

/// <summary>
/// Maps domain <see cref="ResultError"/> (Zalary-shaped, <see cref="ResultType"/>) to application <see cref="Error"/> until the global result type is unified.
/// </summary>
internal static class DomainResultAdapter
{
    public static Error ToApplicationError(ResultError error) =>
        error.Type switch
        {
            ResultType.NotFound => Error.NotFound(error.Message),
            ResultType.Conflicted => Error.Conflict(error.Message),
            ResultType.Forbidden => Error.Failure(ErrorCodes.Forbidden, error.Message),
            ResultType.Unauthorized => Error.Failure(ErrorCodes.Unauthorized, error.Message),
            ResultType.InternalError => Error.Failure(error.Code, error.Message),
            ResultType.Invalid => Error.Validation(error.Message),
            _ => Error.Failure(error.Code, error.Message),
        };
}
