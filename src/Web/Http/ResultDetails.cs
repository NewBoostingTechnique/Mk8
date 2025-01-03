namespace Mk8.Web.Http;

internal static class ResultDetails
{
    internal static string Required(string propertyName)
    {
        return $"{propertyName} is required.";
    }
}
