using Microsoft.AspNetCore.Mvc;

namespace Mk8.Web.App;

public class Api : ControllerBase
{
    protected Api() { }

    protected BadRequestObjectResult BadRequestPropertyRequired(string propertyName)
    {
        return BadRequest($"Property '{propertyName}' is required.");
    }
}