using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Mk8.Core.Logins;
using NSubstitute;
using NSubstitute.ClearExtensions;

namespace Mk8.Web.Test;

public class EndpointTest
{
    protected EndpointTest() { }

    #region HttpClient.

    protected HttpClient HttpClient
    {
        get
        {
            if (httpClientField is null)
            {
                httpClientField = WebApplicationFactory.CreateClient();
                httpClientField.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authSchemeName);
            }
            return httpClientField;
        }
    }
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

    private ILoginStore loginStore => loginStoreField ??= Substitute.For<ILoginStore>();
    private ILoginStore? loginStoreField;

    [SetUp]
    public void SetUpLoginStore()
    {
        loginStoreField?.ClearSubstitute();
    }

    private const string authSchemeName = "Fake";

    protected virtual void ConfigureTestServices(IServiceCollection services)
    {
        AuthenticationBuilder authenticationBuilder = services.AddAuthentication(defaultScheme: authSchemeName);
        authenticationBuilder.AddScheme<AuthenticationSchemeOptions, FakeAuthenticationHandler>(authSchemeName, _ => { });

        services.AddSingleton(loginStore);
    }

    protected void GivenImAuthorized()
    {
        loginStore.ExistsAsync(Arg.Any<string>()).Returns(true);
    }

    protected void GivenImNotAuthorized()
    {
        loginStore.ExistsAsync(Arg.Any<string>()).Returns(false);
    }

    [OneTimeTearDown]
    public void TimeTearDownWebApplicationFactory()
    {
        webApplicationFactoryField?.Dispose();
    }

    #endregion webApplicationFactory.

    #endregion HttpClient.

}
