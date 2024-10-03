using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mk8.Web.App;
using Mk8.Core.Players;
using System.Collections.Immutable;

namespace Mk8.Web.Players;

[Route("api/players")]
public class PlayerApi(
    IPlayerService playerService
) : Api
{
    [HttpPost("")]
    public async Task<IActionResult> CreateAsync([FromBody] Player player)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrWhiteSpace(player?.CountryName))
            return BadRequestPropertyRequired(nameof(Player.CountryName));

        if (string.IsNullOrWhiteSpace(player.Name))
            return BadRequestNameRequired();

        if (await playerService.ExistsAsync(player.Name))
            return Conflict();

        return Ok(await playerService.CreateAsync(player));
    }

    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string name)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrWhiteSpace(name))
            return BadRequestNameRequired();

        if (!await playerService.ExistsAsync(name))
            return NotFound(name);

        await playerService.DeleteAsync(name);

        return Ok();
    }

    [AllowAnonymous, HttpGet("{playerName}")]
    public async Task<IActionResult> DetailAsync([FromRoute] string playerName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrWhiteSpace(playerName))
            return BadRequestNameRequired();

        Player? player = await playerService.DetailAsync(playerName);

        return player is null ? NotFound(playerName) : Ok(player);
    }

    [AllowAnonymous, HttpGet("")]
    public async Task<IActionResult> IndexAsync()
    {
        IImmutableList<Player> players = await playerService.IndexAsync();
        return Ok(players);
    }

    [HttpPost("migrate")]
    public async Task<IActionResult> MigrateAsync()
    {
        return Ok(await playerService.MigrateAsync());
    }

    private BadRequestObjectResult BadRequestNameRequired()
    {
        return BadRequestPropertyRequired(nameof(Player.Name));
    }

    private NotFoundObjectResult NotFound(string name)
    {
        return base.NotFound($"Player '{name}' was not found.");
    }
}
