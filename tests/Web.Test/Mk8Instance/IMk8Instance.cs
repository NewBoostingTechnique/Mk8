namespace Mk8.Web.Test.Mk8Instances;

public interface IMk8Instance : IDisposable
{
    ValueTask<Uri> GetBaseUrlAsync();
}