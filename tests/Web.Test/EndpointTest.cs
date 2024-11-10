using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Mk8.Web.Test;

public class EndpointTest
{
    protected EndpointTest() { }

    #region HttpClient.

    protected HttpClient HttpClient => httpClientField ??= WebApplicationFactory.CreateClient();
    private HttpClient? httpClientField;

    [OneTimeTearDown]
    public void TearDownHttpClient()
    {
        httpClientField?.Dispose();
    }

    #region webApplicationFactory.

    private Mk8WebApplicationFactory WebApplicationFactory => webApplicationFactoryField ??= new Mk8WebApplicationFactory(ConfigureTestServices);
    private Mk8WebApplicationFactory? webApplicationFactoryField;

    private sealed class Mk8WebApplicationFactory(Action<IServiceCollection> configureTestServices) : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(configureTestServices);
        }
    }

    protected virtual void ConfigureTestServices(IServiceCollection services) { }

    [OneTimeTearDown]
    public void TimeTearDownWebApplicationFactory()
    {
        webApplicationFactoryField?.Dispose();
    }

    #endregion webApplicationFactory.

    #endregion HttpClient.

}
