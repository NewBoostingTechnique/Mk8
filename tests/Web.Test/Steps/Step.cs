using Microsoft.Playwright;
using Mk8.Web.Test.Extensions;
using Mk8.Web.Test.Mk8Instances;

namespace Mk8.Web.Test.Steps;

public class Step
{
    private readonly Func<IPage> _pageAccessor;
    protected IPage Page => _pageAccessor();

    protected IMk8Instance Mk8Instance { get; }

    protected Step(Func<IPage> pageAccessor, IMk8Instance mk8Instance)
    {
        _pageAccessor = pageAccessor;
        Mk8Instance = mk8Instance;
    }

    protected async Task GoToPathAsync(string path)
    {
        await Page.GotoAsync
        (
            await Mk8Instance.GetAbsoluteUrlAsync(path)
        );
    }

    protected async Task SignIn(GoogleAccount googleAccount)
    {
        await Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions { Name = "Email or phone" }).FillAsync(googleAccount.Email);
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Next" }).ClickAsync();

        await Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions { Name = "Enter your password" }).FillAsync(googleAccount.Password);
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Next" }).ClickAsync();
    }
}
