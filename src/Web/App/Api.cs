using Microsoft.AspNetCore.Mvc;
using Mk8.Web.Http;

namespace Mk8.Web.App;

public class Api : ControllerBase
{
    protected Api() { }

    protected BadRequestObjectResult BadRequestPropertyRequired(string propertyName)
    {
        return BadRequest(ResultDetails.Required(propertyName));
    }
}
