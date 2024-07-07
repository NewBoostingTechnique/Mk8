using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mk8.Web.App;
using Mk8.Core.Players;
using Mk8.Core.ProofTypes;
using System.Collections.Immutable;

namespace Mk8.Web.Players;

[Route("api/player")]
public class PlayerApi(
    IPlayerService playerService,
    IProofTypeService proofService
) : Api
{
    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string name)
    {
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
        if (string.IsNullOrWhiteSpace(playerName))
            return BadRequestNameRequired(); // TODO: Use Problem.

        Player? player = await playerService.FindAsync(playerName);

        return player is null ? NotFound(playerName) : Ok(player);
    }

    [HttpPost("")]
    public async Task<IActionResult> InsertAsync([FromBody] Player player)
    {
        if (string.IsNullOrWhiteSpace(player?.CountryName))
            return BadRequestPropertyRequired(nameof(Player.CountryName));

        if (string.IsNullOrWhiteSpace(player.Name))
            return BadRequestNameRequired();

        if (await playerService.ExistsAsync(player.Name))
            return Conflict();

        if (string.IsNullOrWhiteSpace(player.ProofTypeDescription))
            return BadRequestPropertyRequired(nameof(Player.ProofTypeDescription));

        if (!await proofService.ExistsAsync(player.ProofTypeDescription))
            return NotFound(player.ProofTypeDescription);

        return Ok(await playerService.InsertAsync(player));
    }

    [AllowAnonymous, HttpGet("")]
    public async Task<IActionResult> ListAsync()
    {
        IImmutableList<Player> players = await playerService.ListAsync();
        return Ok(players);
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
