using Microsoft.Extensions.Logging;
using Mk8.Core.Persons;

namespace Mk8.Core.Logins;

internal class LoginService(
    ILogger<LoginService> logger,
    ILoginData loginData,
    IPersonData personData
) : ILoginService
{
    public Task<bool> ExistsAsync(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        return loginData.ExistsAsync(email);
    }

    public async Task SeedAsync()
    {
        logger.LogInformation("Seeding logins...");

        await loginData.InsertAsync(new Login
        {
            Id = Ulid.NewUlid(),
            Email = "russell.horwood@gmail.com",
            PersonId = await personData.IdentifyRequiredAsync("Russell Horwood").ConfigureAwait(false)
        })
        .ConfigureAwait(false);
    }
}
