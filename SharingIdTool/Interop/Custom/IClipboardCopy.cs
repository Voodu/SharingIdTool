namespace SharingIdTool.Interop.Custom;

public interface IClipboardCopy
{
    Task CopyToClipboardAsync(string text);
}