using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mk8.Core.Regions;
using Mk8.Web.App;

namespace Mk8.Web.Regions;

[Route("api/regions")]
public class RegionApi(IRegionService regionService) : Api
{
    [AllowAnonymous, HttpGet("{countryName}")]
    public async Task<IActionResult> IndexAsync([FromRoute] string countryName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrWhiteSpace(countryName))
            return BadRequestPropertyRequired(nameof(countryName));

        return Ok(await regionService.IndexAsync(countryName));
    }
}
