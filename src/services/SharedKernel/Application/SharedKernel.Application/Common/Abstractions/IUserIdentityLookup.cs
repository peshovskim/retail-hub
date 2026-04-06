namespace RetailHub.SharedKernel.Application.Common.Abstractions;

public interface IUserIdentityLookup
{
    Task<int?> GetUserIdByUidAsync(Guid uid, CancellationToken cancellationToken = default);
}
