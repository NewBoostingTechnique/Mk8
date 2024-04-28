using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mk8.Core.News;
using Mk8.Web.App;

namespace Mk8.Web.News;

[Route("api/news")]
public class NewsApi(INewsService newsService) : Api
{
    [AllowAnonymous, HttpGet("")]
    public async Task<IActionResult> ListAsync()
    {
        return Ok(await newsService.ListAsync());
    }
}