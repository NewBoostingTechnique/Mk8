using Mk8.Core.Players;

namespace Mk8.Web.Test.Steps;

internal sealed class PostPlayerResult
{
    internal PostPlayerResult(int httpStatusCode, Player player)
    {
        HttpStatusCode = httpStatusCode;
        Player = player;
    }

    internal Player Player { get; }
    internal int HttpStatusCode { get; }
}