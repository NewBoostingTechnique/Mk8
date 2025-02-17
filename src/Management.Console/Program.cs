using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mk8.Management.MySql;
using Mk8.Management.Console;
using Mk8.MySql;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
builder.Services.AddHostedService<ConsoleService>();
builder.AddMySql();
builder.AddMySqlManagement();

using IHost host = builder.Build();
await host.RunAsync();
