using Microsoft.AspNetCore.HttpOverrides;
using Mk8.Web.Authentication;
using Mk8.Web.Authorization;
using Mk8.Web.Text.Json.Serialization;
using Mk8.MySql;
using Mk8.Core.Extensions;

// TODO: NuGet package caching in GitHub Actions.
// TODO: Run CI tests in GitHub Actions.
// TODO: REST API Routes should be plural.
// TODO: Make the UI Mobile friendly (reactive).

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddAuthentication();
builder.AddAuthorization();
builder.Services.AddMySql();
builder.AddMk8();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new TimeSpanJsonConverter()));

WebApplication app = builder.Build();
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

app.MapFallbackToFile("App/Index.html");
await app.RunAsync();
