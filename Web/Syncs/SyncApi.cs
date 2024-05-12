using Microsoft.AspNetCore.Mvc;
using Mk8.Core.Syncs;
using Mk8.Web.App;

namespace Mk8.Web.Syncs;

[Route("api/sync")]
public class SyncApi(ISyncService syncService) : Api
{
    [HttpPost("")]
    public IActionResult Insert()
    {
        syncService.Insert();
        return Ok();
    }
}