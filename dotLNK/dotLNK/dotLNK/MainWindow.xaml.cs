using dotLNKLibrary;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using static dotLNKLibrary.WindowsShortcut;

namespace dotLNK;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
        {
            UseShellExecute = true,
            FileName = e.Uri.AbsoluteUri,
        });
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    private void TabControl_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    private void ButtonClose_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        string blank = "<!doctype html><html style=\"background-color: #161616; color: #ffffff; font-weight: 600; font-family: 'Segoe UI', sans-serif;\" lang=\"en\"><head><meta charset=\"utf-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"><title>Static Analysis Report</title></head><body><h2 style=\"margin-bottom: 0px; font-weight: normal;\">about:blank</h2><div style=\"padding: 14px;\"><p>Click <span style=\"color: #00aaff;\">Open</span> button and load .lnk file sample for static analysis.</p><p>Malware can try to exploit defensive tools &mdash; always run malware samples in isolated environment. For example: use virtual machine.</p></div></body></html>";
        webBrowser.NavigateToString(blank);
    }

    private static (bool, byte[]) data = (false, []);

    private void Analyze()
    {
        string text = "<!doctype html><html style=\"word-break: break-all; background-color: #161616; color: #ffffff; font-weight: 600; font-family: 'Segoe UI', sans-serif;\" lang=\"en\"><head><meta charset=\"utf-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"><title>Static Analysis Report</title></head><body>";

        if (data.Item1 == true)
        {
            Data = data.Item2;

            text += $"<p>dotLNK is Windows Shortcut (.lnk) Analysis Tool<br />by ethical<span style=\"color: #0094ff;\">.blue</span> Magazine // Cybersecurity clarified.</p>";

            text += $"<h2 style=\"margin-bottom: 0px; font-weight: normal;\">About Sample</h2>";

            text += "<div style=\"padding-left: 14px;\">";

            text += $"FileName: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(WindowsShortcutReader.FileName)}</span><br />";

            long fileSize = new System.IO.FileInfo(HttpUtility.HtmlEncode(WindowsShortcutReader.FileName)).Length;

            if(fileSize / 1024 >= 100)
                text += $"FileSize: <span style=\"color: #ff0040;\">{fileSize} bytes ({fileSize / 1024} KB)</span><br />";
            else
                text += $"FileSize: <span style=\"color: #0097ff;\">{fileSize} bytes ({fileSize / 1024} KB)</span><br />";

            double entropy = Misc.Entropy(Data);

            if (entropy >= 6.4)
                text += $"Entropy: <span style=\"color: #ff0040;\">{entropy:0.##}</span><br />";
            else
                text += $"Entropy: <span style=\"color: #0097ff;\">{entropy:0.##}</span><br />";

            var hashSHA256 = SHA256.HashData(Data);
            string sha256Text = BitConverter.ToString(hashSHA256).Replace("-", "");
            text += $"SHA-256: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(sha256Text.ToLowerInvariant())}</span><br />";

            var hashSHA512 = SHA512.HashData(Data);
            string sha512Text = BitConverter.ToString(hashSHA512).Replace("-", "");

            text += $"SHA-512: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(sha512Text.ToLowerInvariant())}</span>";

            text += "</div>";

            text += $"<h2 style=\"margin-bottom: 0px; font-weight: normal;\">ShellLinkHeader</h2>";
            text += "<div style=\"padding-left: 14px;\">";

            text += "<p>";

            text += $"HeaderSize: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(ShellLinkHeader.HeaderSize.ValueAsText)}</span><br />";
            if (ShellLinkHeader.HeaderSize.IsValid)
                text += $"HeaderSize.IsValid: <span style=\"color: #0097ff;\">True</span>";
            else
                text += $"HeaderSize.IsValid: <span style=\"color: #ff0040;\">False</span>";

            text += "</p>";
            text += "<p>";

            text += $"LinkCLSID: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(ShellLinkHeader.LinkCLSID.ValueAsText)}</span><br />";
            if (ShellLinkHeader.LinkCLSID.IsValid)
                text += $"LinkCLSID.IsValid: <span style=\"color: #0097ff;\">True</span>";
            else
                text += $"LinkCLSID.IsValid: <span style=\"color: #ff0040;\">False</span>";

            text += "</p>";
            text += "<p>";

            text += $"FileAttributesFlags: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(ShellLinkHeader.FileAttributesFlags.ValuesAsText())}</span>";

            text += "</p>";
            text += "<p>";

            text += $"CreationTime: <span style=\"color: #0097ff;\">{ShellLinkHeader.CreationTime.DateTimeValue().ToUniversalTime()}</span><br />";
            if (ShellLinkHeader.CreationTime.IsZero)
                text += $"CreationTime.IsWiped: <span style=\"color: #ff0040;\">True</span><br />";
            else
                text += $"CreationTime.IsWiped: <span style=\"color: #0097ff;\">False</span><br />";

            text += $"AccessTime: <span style=\"color: #0097ff;\">{ShellLinkHeader.AccessTime.DateTimeValue().ToUniversalTime()}</span><br />";
            if (ShellLinkHeader.AccessTime.IsZero)
                text += $"AccessTime.IsWiped: <span style=\"color: #ff0040;\">True</span><br />";
            else
                text += $"AccessTime.IsWiped: <span style=\"color: #0097ff;\">False</span><br />";

            text += $"WriteTime: <span style=\"color: #0097ff;\">{ShellLinkHeader.WriteTime.DateTimeValue().ToUniversalTime()}</span><br />";
            if (ShellLinkHeader.WriteTime.IsZero)
                text += $"WriteTime.IsWiped: <span style=\"color: #ff0040;\">True</span>";
            else
                text += $"WriteTime.IsWiped: <span style=\"color: #0097ff;\">False</span>";

            text += "</p>";
            text += "<p>";

            text += $"FileSize: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(ShellLinkHeader.FileSize.ValueAsText)}</span><br />";
            text += $"FileSize.Warning: <span style=\"color: #aaaaaa;\">{HttpUtility.HtmlEncode(ShellLinkHeader.FileSize.Warning)}</span>";

            text += "</p>";
            text += "<p>";

            text += $"IconIndex: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(ShellLinkHeader.IconIndex.ValueAsInt)}</span>";

            text += "</p>";
            text += "<p>";

            text += $"ShowCommand: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(ShellLinkHeader.ShowCommand.ValueAsText())}</span>";

            text += "</p>";
            text += "<p>";

            text += $"HotKey.LowByte: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(ShellLinkHeader.HotKey.LowByteAsText())}</span><br />";
            text += $"HotKey.HighByte: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(ShellLinkHeader.HotKey.HighByteAsText())}</span>";

            text += "</p>";
            text += "<p>";

            text += $"Reserved1: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(ShellLinkHeader.Reserved1.ValueAsText)}</span><br />";
            text += $"Reserved2: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(ShellLinkHeader.Reserved2.ValueAsText)}</span><br />";
            text += $"Reserved3: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(ShellLinkHeader.Reserved3.ValueAsText)}</span>";

            text += "</p>";
            text += "</div>";

            text += $"<h2 style=\"margin-bottom: 0px; font-weight: normal;\">LinkTargetIDList</h2>";

            if (LinkTargetIDList.Exists)
            {
                text += "<div style=\"padding-left: 14px;\">";

                foreach (var item in LinkTargetIDList.IDList.ItemsAsHtml())
                {
                    text += "<p>" + item + "</p>";
                }

                text += "</div>";
            }
            else
            {
                text += "<div style=\"padding-left: 14px;\"><p>LinkTargetIDList.Exists: False</p></div>";
            }

            text += $"<h2 style=\"margin-bottom: 0px; font-weight: normal;\">LinkInfo</h2>";

            if (LinkInfo.Exists)
            {
                text += "<div style=\"padding-left: 14px;\">";

                text += "<p>";

                text += $"LinkInfoSize: <span style=\"color: #0097ff;\">{LinkInfo.LinkInfoSize.ValueAsUInt} ({HttpUtility.HtmlEncode(LinkInfo.LinkInfoSize.ValueAsUInt.AsHex())})</span><br />";
                text += $"LinkInfoHeaderSize: <span style=\"color: #0097ff;\">{LinkInfo.LinkInfoHeaderSize.ValueAsUInt} ({HttpUtility.HtmlEncode(LinkInfo.LinkInfoHeaderSize.ValueAsUInt.AsHex())})</span><br />";
                text += $"LinkInfoFlags: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(LinkInfo.LinkInfoFlags.ValuesAsText())}</span><br />";
                text += $"Information: <span style=\"color: #aaaaaa;\">{HttpUtility.HtmlEncode(LinkInfo.LinkInfoHeaderSize.Information())}</span>";

                text += "</p>";
                text += "</div>";

                text += $"<h3 style=\"margin-bottom: 0px; font-weight: normal;\">LinkInfo.VolumeID</h3>";

                text += "<div style=\"padding-left: 14px;\">";

                text += "<p>";

                text += $"VolumeID.DriveType: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(LinkInfo.VolumeID.DriveType())}</span><br />";
                text += $"VolumeID.DriveSerialNumber: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(LinkInfo.VolumeID.DriveSerialNumber())}</span><br />";
                text += $"VolumeID.VolumeLabel: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(LinkInfo.VolumeID.VolumeLabel())}</span><br />";
                text += $"VolumeID.VolumeLabelUnicode: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(LinkInfo.VolumeID.VolumeLabelUnicode())}</span><br />";
                text += $"LocalBasePath: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(LinkInfo.LocalBasePath.Value())}</span><br />";
                text += $"LocalBasePathUnicode: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(LinkInfo.LocalBasePathUnicode.Value())}</span>";

                text += "</p>";

                text += "</div>";
            }
            else
            {
                text += "<div style=\"padding-left: 14px;\"><p>LinkInfo.Exists: False</p></div>";
            }

            text += $"<h3 style=\"margin-bottom: 0px; font-weight: normal;\">LinkInfo.CommonNetworkRelativeLink</h3>";

            if (LinkInfo.CommonNetworkRelativeLink.Exists)
            {
                text += "<div style=\"padding-left: 14px;\">";

                text += "<p>";

                text += $"CommonNetworkRelativeLinkSize: <span style=\"color: #0097ff;\">{LinkInfo.CommonNetworkRelativeLink.CommonNetworkRelativeLinkSize.ValueAsUInt} ({HttpUtility.HtmlEncode(LinkInfo.CommonNetworkRelativeLink.CommonNetworkRelativeLinkSize.ValueAsUInt.AsHex())})</span><br />";
                text += $"CommonNetworkRelativeLinkFlags: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(LinkInfo.CommonNetworkRelativeLink.CommonNetworkRelativeLinkFlags.ValuesAsText())}</span><br />";
                text += $"NetworkProviderType: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(LinkInfo.CommonNetworkRelativeLink.NetworkProviderType().Item1)} ({HttpUtility.HtmlEncode(LinkInfo.CommonNetworkRelativeLink.NetworkProviderType().Item2)})</span><br />";
                text += $"NetName: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(LinkInfo.CommonNetworkRelativeLink.NetName())}</span><br />";
                text += $"NetNameUnicode: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(LinkInfo.CommonNetworkRelativeLink.NetNameUnicode())}</span><br />";
                text += $"DeviceName: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(LinkInfo.CommonNetworkRelativeLink.DeviceName())}</span><br />";
                text += $"DeviceNameUnicode: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(LinkInfo.CommonNetworkRelativeLink.DeviceNameUnicode())}</span>";

                text += "</p>";

                text += "</div>";
            }
            else
            {
                text += "<div style=\"padding-left: 14px;\"><p>CommonNetworkRelativeLink.Exists: False</p></div>";
            }

            text += $"<h2 style=\"margin-bottom: 0px; font-weight: normal;\">StringData</h2>";

            text += "<div style=\"padding-left: 14px;\">";

            foreach (var item in StringData.Items())
            {
                text += $"{HttpUtility.HtmlEncode(item.Item1)}: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(item.Item2)}</span><br />";
            }

            text += "</div>";

            text += $"<h2 style=\"margin-bottom: 0px; font-weight: normal;\">ExtraData</h2>";

            text += "<div style=\"padding-left: 14px;\">";

            foreach (var item in ExtraData.ExtractEssentialData())
            {
                if (string.IsNullOrEmpty(item.name))
                    text += "<br />";
                else
                    text += $"{HttpUtility.HtmlEncode(item.Item1)}: <span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(item.Item2)}</span><br />";
            }

            text += "</div>";

            text += $"<h2 style=\"margin-bottom: 0px; font-weight: normal;\">ExtraData.TextStrings</h2>";

            text += "<div style=\"padding-left: 14px;\">";

            foreach (var item in ExtraData.TextStrings())
            {
                if (string.IsNullOrEmpty(item))
                    text += "<br />";
                else
                    text += $"<span style=\"color: #0097ff;\">{HttpUtility.HtmlEncode(item)}</span><br />";
            }

            text += "</div>";

            text += "</body></html>";

            Application.Current.Dispatcher.Invoke(new Action(() => { webBrowser.NavigateToString(text); buttonAnalyze.IsEnabled = true; }));
        }
    }
    private void buttonAnalyze_Click(object sender, RoutedEventArgs e)
    {
        data = WindowsShortcutReader.OpenFile();
        buttonAnalyze.IsEnabled = false;

        string working = "<!doctype html><html style=\"background-color: #161616; color: #ffffff; font-weight: 600; font-family: 'Segoe UI', sans-serif;\" lang=\"en\"><head><meta charset=\"utf-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"><title>Static Analysis Report</title></head><body><h2 style=\"margin-bottom: 0px; font-weight: normal;\">Analysis in progress...</h2></body></html>";
        webBrowser.NavigateToString(working);

        Thread backgroundThread = new(new ThreadStart(Analyze));
        backgroundThread.Start();
    }
}