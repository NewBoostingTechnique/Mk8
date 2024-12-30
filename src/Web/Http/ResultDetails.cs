namespace Mk8.Web.Http;

internal static class ResultDetails
{
    public static string Required(string propertyName)
    {
        return $"{propertyName} is required.";
    }
}
