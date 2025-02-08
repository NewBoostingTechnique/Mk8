using System.CommandLine;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Hosting;
using Mk8.Management.Core.Deployments.Create;
using Mk8.Management.Core.Deployments.Seed;

namespace Mk8.Management.Console;

internal partial class ConsoleService(
    IHostApplicationLifetime hostApplicationLifetime,
    ICommandHandler<CreateDeploymentCommand, Result> createDeploymentCommand,
    ICommandHandler<SeedDeploymentCommand, Result> seedDeploymentCommand
) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        hostApplicationLifetime.ApplicationStarted.Register(async () =>
        {
            RootCommand rootCommand = new("MK8 MySQL Console");

            Command deploy = new("deploy");
            rootCommand.Add(deploy);
            deploy.SetHandler
            (
                () => createDeploymentCommand.Handle
                (
                    new CreateDeploymentCommand { Name = "mk8" },
                    cancellationToken
                )
            );

            Command seed = new("seed");
            rootCommand.Add(seed);
            seed.SetHandler
            (
                () => seedDeploymentCommand.Handle
                (
                    new SeedDeploymentCommand(),
                    cancellationToken
                )
            );

            rootCommand.SetHandler(deploy.Handler!.InvokeAsync);

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
