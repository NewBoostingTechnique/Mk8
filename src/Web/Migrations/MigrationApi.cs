using Microsoft.AspNetCore.Mvc;
using Mk8.Core.Migrations;
using Mk8.Web.App;

namespace Mk8.Web.Migrations;

[Route("api/migrations")]
public class MigrationApi(IMigrationService migrationService) : Api
{
    [HttpPost("")]
    public async Task<IActionResult> CreateAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Accepted(await migrationService.CreateAsync(cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> DetailAsync([FromRoute] Ulid? id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!id.HasValue)
            return BadRequestPropertyRequired(nameof(id));

        Migration? migration = await migrationService.DetailAsync(id.Value, cancellationToken);

        return migration is null ? NotFound(id) : Ok(migration);
    }

    [HttpGet("")]
    public async Task<IActionResult> IndexAsync([FromQuery] Migration? after, CancellationToken cancellationToken)
    {
        return Ok
        (
            await migrationService.IndexAsync(after, cancellationToken)
        );
    }
}
