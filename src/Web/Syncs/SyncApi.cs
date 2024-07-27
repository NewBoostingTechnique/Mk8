using Microsoft.AspNetCore.Mvc;
using Mk8.Core.Syncs;
using Mk8.Web.App;

namespace Mk8.Web.Syncs;

[Route("api/syncs")]
public class SyncApi(ISyncService syncService) : Api
{
    [HttpGet("{syncId}")]
    public async Task<IActionResult> DetailAsync([FromRoute] Ulid? syncId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!syncId.HasValue)
            return BadRequestPropertyRequired(nameof(syncId));

        Sync? sync = await syncService.FindAsync(syncId.Value);

        return sync is null ? NotFound(syncId) : Ok(sync);
    }

    [HttpPost("")]
    public async Task<IActionResult> InsertAsync([FromBody] Sync sync)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(await syncService.InsertAsync(sync));
    }
}
