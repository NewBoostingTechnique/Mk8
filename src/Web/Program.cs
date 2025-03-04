using Microsoft.AspNetCore.HttpOverrides;
using Mk8.Core;
using Mk8.Web.Authentication;
using Mk8.Web.Authorization;
using Mk8.Web.Text.Json.Serialization;
using Mk8.MySql;
using Mk8.Web.Times.Create;

namespace Mk8.Web;

public class Program
{
    protected Program() { }

    private static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.AddAuthentication();
        builder.AddAuthorization();
        builder.Services.AddMySql();
        builder.Services.AddMk8Core();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new TimeSpanJsonConverter()));

        using WebApplication app = builder.Build();
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();
        else
            app.UseExceptionHandler("/Api/Error");
        app.UseStaticFiles();
        app.UseAuthorization();
        app.MapControllers().RequireAuthorization();
        app.MapCreateTimeApi();

        app.MapFallbackToFile("App/Index.html");
        await app.RunAsync();
    }
}
