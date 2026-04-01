namespace RetailHub.BuildingBlocks.Application.Common.Results;

public sealed record Error(string Code, string Message)
{
    public static Error Validation(string message) => new(ErrorCodes.Validation, message);

    public static Error NotFound(string message) => new(ErrorCodes.NotFound, message);

    public static Error Conflict(string message) => new(ErrorCodes.Conflict, message);

    public static Error Failure(string code, string message) => new(code, message);
}
