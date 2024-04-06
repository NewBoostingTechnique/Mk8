using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mk8.Core.Locations.Countries;
using Mk8.Web.App;

namespace Mk8.Web.Locations.Countries;

[Route("api/country")]
public class CountryApi(ICountryService countryService) : Api
{
    [AllowAnonymous, HttpGet("")]
    public async Task<IActionResult> ListAsync()
    {
        return Ok(await countryService.ListAsync());
    }
}
