using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Players;
using NSubstitute;

namespace Mk8.Web.Test.PlayerApiTest;

public class PlayerApiEndpointTest : EndpointTest
{
    protected IPlayerStore PlayerStore { get; } = Substitute.For<IPlayerStore>();

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        base.ConfigureTestServices(services);
        services.AddSingleton(PlayerStore);
    }
}
