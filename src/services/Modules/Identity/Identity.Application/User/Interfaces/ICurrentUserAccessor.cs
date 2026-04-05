namespace Identity.Application.User.Interfaces;

public interface ICurrentUserAccessor
{
    /// <summary>
    /// Snapshot of the authenticated user from the current request claims, or <c>null</c> if unavailable.
    /// </summary>
    CurrentUserSnapshot? GetSnapshot();
}

public sealed record CurrentUserSnapshot(Guid UserId, string? Email, IReadOnlyList<string> Roles);
