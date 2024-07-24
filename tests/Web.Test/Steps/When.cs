using Microsoft.Playwright;
using Mk8.Core.Players;
using Mk8.Core.ProofTypes;
using Mk8.Web.Test.Extensions;
using Mk8.Web.Test.Mk8Instances;

namespace Mk8.Web.Test.Steps;

public class When(Func<IPage> pageAccessor, IMk8Instance softwareUnderTest)
    : Step(pageAccessor, softwareUnderTest)
{
    internal Task INavigateToAsync(string path)
    {
        return GoToPathAsync(path);
    }

    internal async Task<PostPlayerResult> IPostANewPlayerToAsync(string path)
    {
        Player player = new()
        {
            Name = Guid.NewGuid().ToString(),
            CountryName = Guid.NewGuid().ToString(),
            ProofTypeId = "1cafdd6d1ef34b35b41e9962051e4c55"
        };

        IAPIResponse response = await Page.APIRequest.PostAsync
        (
            await Mk8Instance.GetAbsoluteUrlAsync(path),
            new APIRequestContextOptions
            {
                DataObject = player,
                IgnoreHTTPSErrors = true
            }
        );

        return new PostPlayerResult(response.Status, player);
    }

    internal Task ISignInAsAnAuthorizedUser()
    {
        return SignIn(GoogleAccount.AuthorizedUser);
    }
}