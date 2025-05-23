using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mk8.Core.Countries;
using Mk8.Core.Courses;
using Mk8.Core.Logins;
using Mk8.Core.Persons;
using Mk8.Core.Players;
using Mk8.Core.Times;
using Mk8.MySql;
using Mk8.Web.Test.Authentication;
using NSubstitute;
using NSubstitute.ClearExtensions;

namespace Mk8.Web.Test;

public class EndpointTest
{
    static EndpointTest()
    {
        Environment.SetEnvironmentVariable("Authentication:Google:ClientId", "MyClientId");
        Environment.SetEnvironmentVariable("Authentication:Google:ClientSecret", "MyClientSecret");
    }

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
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(ConfigureHostConfiguration);
            return base.CreateHost(builder);
        }

        private static void ConfigureHostConfiguration(IConfigurationBuilder builder)
        {
            builder.AddInMemoryCollection
            ([
                new KeyValuePair<string, string?>
                (
                    $"{MySqlSettings.SectionName}:{nameof(MySqlSettings.ConnectionString)}",
                    "not a real connection string"
                )
            ]);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
            builder.ConfigureTestServices(configureTestServices);
        }
    }

    private ILoginStore LoginStore => _loginStore ??= Substitute.For<ILoginStore>();
    private ILoginStore? _loginStore;

    [SetUp]
    public void SetUpLoginStore()
    {
        _loginStore?.ClearSubstitute();
    }

    protected virtual void ConfigureTestServices(IServiceCollection services)
    {
        ConfigureAuthenticationServices(services);
        services.AddSingleton(LoginStore);
        services.AddSingleton(CountryStore);
        services.AddSingleton(CourseStore);
        services.AddSingleton(PersonStore);
        services.AddSingleton(PlayerStore);
        services.AddSingleton(TimeStore);
    }

    #region Authentication.

    private void ConfigureAuthenticationServices(IServiceCollection services)
    {
        AuthenticationBuilder authenticationBuilder = services.AddAuthentication(defaultScheme: authSchemeName);
        authenticationBuilder.AddScheme<AuthenticationSchemeOptions, FakeAuthenticationHandler>(authSchemeName, _ => { });
        services.AddSingleton(authenticator);
    }

    private const string authSchemeName = "Fake";

    private IAuthenticator? authenticatorField;

    private IAuthenticator authenticator => authenticatorField ??= Substitute.For<IAuthenticator>();

    [SetUp]
    public void SetUpAuthenticator()
    {
        authenticatorField?.ClearSubstitute();
    }

    protected void GivenAuthentication()
    {
        Claim[] claims = [new Claim(ClaimTypes.Email, "email@adress.domain")];
        ClaimsIdentity identity = new(claims, authSchemeName);
        ClaimsPrincipal principal = new(identity);
        AuthenticationTicket ticket = new(principal, authSchemeName);
        AuthenticateResult result = AuthenticateResult.Success(ticket);
        authenticator.AuthenticateAsync().Returns(result);
    }

    protected void ArrangeUnauthenticated()
    {
        AuthenticateResult result = AuthenticateResult.Fail("My Failure Message");
        authenticator.AuthenticateAsync().Returns(result);
    }

    #endregion Authentication.

    #region Authorization.

    protected void ArrangeAuthorization() => GivenAuthorization(true);

    protected void GivenAuthorization(bool authorized)
    {
        GivenAuthentication();
        LoginStore.ExistsAsync(Arg.Any<string>()).Returns(authorized);
    }

    protected void ArrangeUnauthorized() => GivenAuthorization(false);

    #endregion Authorization.

    [OneTimeTearDown]
    public void TimeTearDownWebApplicationFactory()
    {
        webApplicationFactoryField?.Dispose();
    }

    #endregion webApplicationFactory.

    #endregion HttpClient.

    #region CountryStore.

    protected ICountryStore CountryStore => _countryStore ??= Substitute.For<ICountryStore>();

    private ICountryStore? _countryStore;

    [SetUp]
    public void SetUpCountryStore()
    {
        _countryStore?.ClearSubstitute();
    }

    #endregion CountryStore.

    #region CourseStore.

    protected ICourseStore CourseStore => _courseStore ??= Substitute.For<ICourseStore>();

    private ICourseStore? _courseStore;

    [SetUp]
    public void SetUpCourseStore()
    {
        _courseStore?.ClearSubstitute();
    }

    #endregion CourseStore.

    #region PersonStore.

    protected IPersonStore PersonStore => _personStore ??= Substitute.For<IPersonStore>();

    private IPersonStore? _personStore;

    [SetUp]
    public void SetUpPersonStore()
    {
        _personStore?.ClearSubstitute();
    }

    #endregion PersonStore.

    #region PlayerStore.

    protected IPlayerStore PlayerStore => _playerStore ??= Substitute.For<IPlayerStore>();

    private IPlayerStore? _playerStore;

    [SetUp]
    public void SetUpPlayerStore()
    {
        _playerStore?.ClearSubstitute();
    }

    #endregion PlayerStore.

    #region TimeStore.

    protected ITimeStore TimeStore => _timeStore ??= Substitute.For<ITimeStore>();

    private ITimeStore? _timeStore;

    [SetUp]
    public void SetUpTimeStore()
    {
        _timeStore?.ClearSubstitute();
    }

    #endregion TimeStore

}
