using Microsoft.JSInterop;

namespace SharingIdTool.Interop.Custom;

public class TeamsThemeChange<T> : InteropModuleBase, IDisposable where T : class
{
    public const string Default = "default";
    public const string Dark = "dark";
    public const string Contrast = "contrast";

    private DotNetObjectReference<T> _objRef;

    public TeamsThemeChange(IJSRuntime jsRuntime) : base(jsRuntime)
    {
    }

    protected override string ModulePath => "./js/ThemeChange.js";

    public void Dispose()
    {
        _objRef?.Dispose();
    }

    public void InitializeInterop(T @class)
    {
        _objRef = DotNetObjectReference.Create(@class);
    }

    public async Task BindOnChangeAsync(string calledMethodName)
    {
        await InvokeVoidAsync("onTeamsThemeChange", _objRef, calledMethodName);
    }

    public async Task UpdateBodyThemeAsync(string theme)
    {
        await InvokeVoidAsync("updateBodyTheme", theme);
    }
}