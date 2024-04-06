using Mk8.Core.Extensions;
using Mk8.Web.Authentication;
using Mk8.Web.Authorization;
using Mk8.Web.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddAuthentication();
builder.AddAuthorization();
builder.AddMk8();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new TimeSpanJsonConverter()));

WebApplication app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandler("/Api/Error");
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers().RequireAuthorization();

app.MapFallbackToFile("App/Index.html");
app.Run();
