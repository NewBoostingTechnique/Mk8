namespace Mk8.Web.Test;

public class GoogleAccount
{
    internal string Email { get; }

    internal string Name { get; }

    internal string Password { get; }

    internal GoogleAccount(string email, string name, string password)
    {
        Email = email;
        Name = name;
        Password = password;
    }

    internal static GoogleAccount AuthorizedUser = new
    (
        "mk8.test.authorized@gmail.com",
        "Mk8 Authorized",
        "dhPtuD:42,(0"
    );

    internal static GoogleAccount Unauthorized = new
    (
        "mk8.test.unauthorized@gmail.com",
        "Mk8 Unauthorized",
        "xErf3ys12h]R"
    );
}