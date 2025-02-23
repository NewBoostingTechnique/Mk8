using System.Reflection;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mk8.Management.Core.Deployments.Create;
using Mk8.Management.Core.Deployments.Delete;
using Mk8.Management.MySql;

namespace Mk8.MySql.Test;

public class MySqlTest
{
    protected MySqlTest() { }

    #region DeploymentName.

    private string DeploymentName => _deploymentName ??= Ulid.NewUlid().ToString();

    private string? _deploymentName;

    #endregion DeploymentName.

    #region Host.

    private IHost Host
    {
        get
        {
            if (_host is null)
            {
                string targetConnectionString = Settings.GetTargetConnectionString(DeploymentName);
                HostApplicationBuilder hostApplicationBuilder = HostApplicationBuilder;
                hostApplicationBuilder.Configuration[$"{MySqlSettings.SectionName}:{nameof(MySqlSettings.ConnectionString)}"] = targetConnectionString;
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

    #region Settings.

    private MySqlManagementSettings Settings
    {
        get
        {
            if (_settings is null)
            {
                _settings = HostApplicationBuilder.Configuration
                   .GetRequiredSection(MySqlManagementSettings.SectionName)
                   .Get<MySqlManagementSettings>()
                   ?? throw new InvalidOperationException("Failed to get the MySQL management settings.");
            }
            return _settings;
        }
    }

    private MySqlManagementSettings? _settings;

    #endregion Settings;

    #region ServiceProviderTask.

    protected Task<IServiceProvider> ServiceProviderTask
    {
        get
        {
            if (_serviceProviderTask is null)
            {
                IHost host = Host;
                ICommandHandler<CreateDeploymentCommand, Result> createDeploymentHandler = host.Services.GetRequiredService<ICommandHandler<CreateDeploymentCommand, Result>>();
                _serviceProviderTask = createDeploymentHandler.Handle(new() { Name = DeploymentName }, CancellationToken.None)
                    .ContinueWith(antecedent => antecedent.IsFaulted ? throw antecedent.Exception.InnerException! : host.Services);
            }
            return _serviceProviderTask;
        }
    }

    private Task<IServiceProvider>? _serviceProviderTask;

    [TearDown]
    public Task TearDownDatabaseAsync()
    {
        if (_deploymentName is null)
            return Task.CompletedTask;

        ICommandHandler<DeleteDeploymentCommand, Result> deleteDeploymentHandler = Host.Services.GetRequiredService<ICommandHandler<DeleteDeploymentCommand, Result>>();
        return deleteDeploymentHandler.Handle(new() { Name = DeploymentName }, CancellationToken.None);
    }

    #endregion ServiceProviderTask.

}

