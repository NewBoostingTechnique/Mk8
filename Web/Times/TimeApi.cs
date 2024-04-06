using Microsoft.AspNetCore.Mvc;
using Mk8.Core.Courses;
using Mk8.Core.Players;
using Mk8.Core.Times;
using Mk8.Web.App;

namespace Mk8.Web.Times;

[Route("api/time")]
public class TimeApi(
    ICourseService courseService,
    IPlayerService playerService,
    ITimeService timeService
) : Api
{
    [HttpPost("")]
    public async Task<IActionResult> InsertAsync([FromBody] Time time)
    {
        if (string.IsNullOrWhiteSpace(time?.CourseName))
            return BadRequestPropertyRequired(nameof(Time.CourseName));

        if (string.IsNullOrWhiteSpace(time.PlayerName))
            return BadRequestPropertyRequired(nameof(Time.PlayerName));

        if (time.Span is null)
            return BadRequestPropertyRequired(nameof(Time.Span));

        if (!await courseService.ExistsAsync(time.CourseName))
            return BadRequest($"Course '{time.CourseName}' does not exist.");

        if (!await playerService.ExistsAsync(time.PlayerName))
            return BadRequest($"Player '{time.PlayerName}' does not exist.");

        if (await timeService.ExistsAsync(time.CourseName, time.PlayerName))
            return BadRequest($"Player '{time.PlayerName}' already has a time for course '{time.CourseName}'.");

        return Ok(await timeService.InsertAsync(time));
    }
}