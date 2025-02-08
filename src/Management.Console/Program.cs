using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mk8.Core;
using Mk8.Management.MySql;
using Mk8.Management.Console;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
builder.Services.AddHostedService<ConsoleService>();
builder.AddMk8Core();
builder.AddMySqlManagement();

using IHost host = builder.Build();
await host.RunAsync();
