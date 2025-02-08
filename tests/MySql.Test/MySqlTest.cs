using System.Reflection;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mk8.Management.Core.Deployments.Create;
using Mk8.Management.MySql;
using MySql.Data.MySqlClient;

namespace Mk8.MySql.Test;

public class MySqlTest
{
    protected MySqlTest() { }

    #region ConnectionStringBuilder.

    private MySqlConnectionStringBuilder ConnectionStringBuilder
    {
        get
        {
            if (_connectionStringBuilder is null)
            {
                _connectionStringBuilder = new(RootConnectionString)
                {
                    Database = DatabaseName
                };
            }
            return _connectionStringBuilder;
        }
    }

    private MySqlConnectionStringBuilder? _connectionStringBuilder;

    #endregion ConnectionStringBuilder.

    #region DatabaseName.

    private string DatabaseName => _databaseName ??= Ulid.NewUlid().ToString();

    private string? _databaseName;

    #endregion DatabaseName.

    #region Host.

    private IHost Host
    {
        get
        {
            if (_host is null)
            {
                MySqlConnectionStringBuilder connectionStringBuilder = ConnectionStringBuilder;
                HostApplicationBuilder hostApplicationBuilder = HostApplicationBuilder;
                hostApplicationBuilder.Configuration[$"{MySqlSettings.SectionName}:{nameof(MySqlSettings.ConnectionString)}"] = connectionStringBuilder.ConnectionString;
                hostApplicationBuilder.AddMySql();
                hostApplicationBuilder.AddMySqlManagement();
                _host = hostApplicationBuilder.Build();
            }
            return _host;
        }
    }

    private IHost? _host;

    [TearDown]
    public void TearDownHost() => _host?.Dispose();

    #endregion Host.

    #region HostApplicationBuilder.

    private HostApplicationBuilder HostApplicationBuilder
    {
        get
        {
            if (_hostApplicationBuilder is null)
            {
                _hostApplicationBuilder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder(Environment.GetCommandLineArgs());
                _hostApplicationBuilder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
            }
            return _hostApplicationBuilder;
        }
    }

    private HostApplicationBuilder? _hostApplicationBuilder;

    #endregion HostApplicationBuilder.

    #region RootConnectionString.

    private string RootConnectionString
    {
        get
        {
            if (_rootConnectionString is null)
            {
                MySqlManagementSettings? settings = HostApplicationBuilder.Configuration
                    .GetRequiredSection(MySqlSettings.SectionName)
                    .Get<MySqlManagementSettings>()
                    ?? throw new InvalidOperationException("Failed to get the MySQL management settings.");

                _rootConnectionString = settings.RootConnectionString;
            }
            return _rootConnectionString;
        }
    }

    private string? _rootConnectionString;

    #endregion RootConnectionString.

    #region ServiceProviderTask.

    protected Task<IServiceProvider> ServiceProviderTask
    {
        get
        {
            if (_serviceProviderTask is null)
            {
                IHost host = Host;
                ICommandHandler<CreateDeploymentCommand, Result> createDeploymentHandler = host.Services.GetRequiredService<ICommandHandler<CreateDeploymentCommand, Result>>();
                _serviceProviderTask = createDeploymentHandler.Handle(new CreateDeploymentCommand { Name = DatabaseName }, CancellationToken.None)
                    .ContinueWith(antecedent => antecedent.IsFaulted ? throw antecedent.Exception.InnerException! : host.Services);
            }
            return _serviceProviderTask;
        }
    }

    private Task<IServiceProvider>? _serviceProviderTask;

    [TearDown]
    public async Task TearDownDatabaseAsync()
    {
        if (_databaseName is null)
            return;

        using MySqlConnection connection = new(RootConnectionString);
        using MySqlCommand command = new($"drop database {DatabaseName};", connection);
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    #endregion ServiceProviderTask.

}

