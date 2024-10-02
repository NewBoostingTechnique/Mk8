using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mk8.Core.Countries;
using Mk8.Web.App;

namespace Mk8.Web.Countries;

[Route("api/countries")]
public class CountryApi(ICountryService countryService) : Api
{
    [AllowAnonymous, HttpGet("")]
    public async Task<IActionResult> IndexAsync()
    {
        return Ok(await countryService.IndexAsync());
    }
}
