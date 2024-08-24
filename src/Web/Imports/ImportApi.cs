using Microsoft.AspNetCore.Mvc;
using Mk8.Core.Imports;
using Mk8.Web.App;

namespace Mk8.Web.Imports;

[Route("api/imports")]
public class ImportApi(IImportService importService) : Api
{
    [HttpGet("{id}")]
    public async Task<IActionResult> DetailAsync([FromRoute] Ulid? id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!id.HasValue)
            return BadRequestPropertyRequired(nameof(id));

        Import? import = await importService.FindAsync(id.Value);

        return import is null ? NotFound(id) : Ok(import);
    }

    [HttpPost("")]
    public async Task<IActionResult> InsertAsync([FromBody] Import import)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Accepted(await importService.InsertAsync(import));
    }
}
