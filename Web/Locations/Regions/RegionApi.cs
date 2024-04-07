using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mk8.Core.Locations.Regions;
using Mk8.Web.App;

namespace Mk8.Web.Locations.Regions;

[Route("api/region")]
public class RegionApi(IRegionService regionService) : Api
{
    [AllowAnonymous, HttpGet("{countryName}")]
    public async Task<IActionResult> ListAsync([FromRoute] string countryName)
    {
        if (string.IsNullOrWhiteSpace(countryName))
            return BadRequestPropertyRequired(nameof(countryName));

        return Ok(await regionService.ListAsync(countryName));
    }
}
