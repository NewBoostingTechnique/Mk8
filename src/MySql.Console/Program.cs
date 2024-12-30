using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mk8.Core;
using Mk8.MySql;
using Mk8.MySql.Console;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
builder.Services.AddHostedService<ConsoleService>();
builder.Services.AddMySql();
builder.Services.AddSingleton<DeployCommand>();
builder.AddMk8Core();

IHost host = builder.Build();
await host.RunAsync();
