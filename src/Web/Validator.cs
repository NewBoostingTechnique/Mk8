using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Mk8.Web.Http;

namespace Mk8.Web;

internal class Validator
{
    internal Dictionary<string, string[]>? Errors { get; private set; }

    internal void Require([NotNullWhen(true)] object? value, [CallerArgumentExpression(nameof(value))] string? name = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (value is null)
            AddError(name, ResultDetails.Required(name));
    }

    internal void Require([NotNullWhen(true)] string? value, [CallerArgumentExpression(nameof(value))] string? name = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (string.IsNullOrWhiteSpace(value))
            AddError(name, ResultDetails.Required(name));
    }

    private void AddError(string name, string detail)
    {
        if (Errors is null)
            Errors = new Dictionary<string, string[]> { [name] = [detail] };
        else if (!Errors.TryAdd(name, [detail]))
            Errors[name] = [.. Errors[name], detail];
    }
}
