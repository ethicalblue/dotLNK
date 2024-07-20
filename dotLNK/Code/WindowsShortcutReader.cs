using Microsoft.Win32;
using System.IO;

namespace dotLNK;

internal sealed partial class WindowsShortcutReader
{
    internal static string FileName = string.Empty;
    internal static (bool, byte[]) OpenFile()
    {
        byte[] data = [];

        try
        {
            OpenFileDialog dialog = new()
            {
                Filter = "All files|*.*",
                Title = "Open Windows Shortcut File (.lnk) to Analyze",
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Multiselect = false,
                DereferenceLinks = false,
                ShowHiddenItems = true
            };

            if (dialog.ShowDialog() == true)
            {
                FileName = dialog.FileName;
                data = File.ReadAllBytes(dialog.FileName);
                return new(true, data);
            }
        }
        catch (Exception)
        {
            return new(false, data);
        }

        return new(false, data);
    }
}
