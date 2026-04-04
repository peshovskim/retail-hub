namespace RetailHub.SharedKernel.Domain;

public enum ResultType
{
    Ok,
    Invalid,
    NotFound,
    Conflicted,
    Forbidden,
    Unauthorized,
    InternalError,
}
