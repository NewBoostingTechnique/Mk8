using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mk8.Core.Courses;
using Mk8.Web.App;

namespace Mk8.Web.Locations.Courses;

[Route("api/course")]
public class CourseApi(ICourseService courseService) : Api
{
    [AllowAnonymous, HttpGet("")]
    public async Task<IActionResult> ListAsync()
    {
        return Ok(await courseService.IndexAsync());
    }
}
