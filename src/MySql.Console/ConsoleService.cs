using System.CommandLine;
using Microsoft.Extensions.Hosting;
using Mk8.Core.Seeds;

namespace Mk8.MySql.Console;

internal partial class ConsoleService(
    IHostApplicationLifetime hostApplicationLifetime,
    ISeedService seedService,
    DeployCommand deployCommand
) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        hostApplicationLifetime.ApplicationStarted.Register(async () =>
        {
            RootCommand rootCommand = new("MK8 MySQL Console");
            rootCommand.SetHandler(deployCommand.ExecuteAsync);

            Command deploy = new("deploy");
            deploy.SetHandler(deployCommand.ExecuteAsync);
            rootCommand.Add(deploy);

            Command seed = new("seed");
            seed.SetHandler(seedService.InsertAsync);
            rootCommand.Add(seed);

            string[] args = Environment.GetCommandLineArgs().Skip(1).ToArray();
            await rootCommand.InvokeAsync(args);

            hostApplicationLifetime.StopApplication();
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
