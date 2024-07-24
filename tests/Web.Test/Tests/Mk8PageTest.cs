using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Mk8.Web.Test.Mk8Instances;
using Mk8.Web.Test.Steps;

namespace Mk8.Web.Test.Tests;

public class Mk8PageTest : PageTest
{
    protected Given Given { get; }

    protected When When { get; }

    protected Then Then { get; }

    private readonly ServiceProvider _serviceProvider;

    protected Mk8PageTest()
    {
        _serviceProvider = new ServiceCollection()
            .AddSingleton<Func<IPage>>(sp => () => Page)
            .AddSingleton<IMk8Instance, Mk8Instance>()
            .BuildServiceProvider();

        Given = ActivatorUtilities.CreateInstance<Given>(_serviceProvider);
        When = ActivatorUtilities.CreateInstance<When>(_serviceProvider);
        Then = ActivatorUtilities.CreateInstance<Then>(_serviceProvider);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDownAsync()
    {
        await _serviceProvider.DisposeAsync();
    }
}