using Microsoft.JSInterop;

namespace SharingIdTool.Interop.Custom;

public class ClipboardCopy : InteropModuleBase, IClipboardCopy
{
    protected override string ModulePath => "./js/ClipboardCopy.js";
    public ClipboardCopy(IJSRuntime jsRuntime) : base(jsRuntime) { }

    public Task CopyToClipboardAsync(string text)
    {
        return InvokeVoidAsync("copyToClipboard", text);
    }
}