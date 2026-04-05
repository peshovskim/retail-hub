namespace Identity.Application.User.Requests;

public sealed record LoginRequest(string Email, string Password);
