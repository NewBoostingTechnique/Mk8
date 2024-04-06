using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mk8.Web.App;

[Route("api/error")]
public class ErrorApi : Api
{
    [AllowAnonymous, HttpGet("")]
    public IActionResult Detail()
    {
        return Problem();
    }
}