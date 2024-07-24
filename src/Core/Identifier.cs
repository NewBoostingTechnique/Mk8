namespace Mk8.Core;

internal static class Identifier
{
    public static string Generate()
    {
        return Guid.NewGuid().ToString("N");
    }
}
