using Microsoft.AspNetCore.Mvc;
using Mk8.Core.Migrations;
using Mk8.Web.App;

namespace Mk8.Web.Migrations;

[Route("api/migrations")]
public class MigrationApi(IMigrationService migrationService) : Api
{
    [HttpPost("")]
    public async Task<IActionResult> CreateAsync()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Accepted(await migrationService.CreateAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> DetailAsync([FromRoute] Ulid? id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!id.HasValue)
            return BadRequestPropertyRequired(nameof(id));

        Migration? migration = await migrationService.DetailAsync(id.Value);

        return migration is null ? NotFound(id) : Ok(migration);
    }

    [HttpGet("")]
    public async Task<IActionResult> IndexAsync()
    {
        return Ok(await migrationService.IndexAsync());
    }
}
