using Mk8.Web.Test.Mk8Instances;

namespace Mk8.Web.Test.Extensions;

public static class Mk8InstanceExtensions
{
    public static async ValueTask<string> GetAbsoluteUrlAsync(this IMk8Instance mk8Instance, string path)
    {
        return new Uri
        (
            await mk8Instance.GetBaseUrlAsync(),
            path
        )
        .AbsoluteUri;
    }
}