namespace Mk8.Core.Logins;

public class Login
{
    public Ulid? Id { get; init; }

    public string? Email { get; init; }

    public required Ulid PersonId { get; init; }
}
