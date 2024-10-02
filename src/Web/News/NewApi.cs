using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mk8.Core.News;
using Mk8.Web.App;

namespace Mk8.Web.News;

[Route("api/news")]
public class NewsApi(INewService newService) : Api
{
    [AllowAnonymous, HttpGet("")]
    public async Task<IActionResult> IndexAsync()
    {
        return Ok(await newService.IndexAsync());
    }

    [HttpPost("migrate")]
    public async Task<IActionResult> MigrateAsync()
    {
        return Ok(await newService.MigrateAsync());
    }
}
