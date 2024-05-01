using dotLNKLibrary;
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
        string blank = "<!doctype html><html style=\"background-color: #000000; color: #ffffff; font-family: sans-serif;\" lang=\"en\"><head><meta charset=\"utf-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"><title>Static Analysis Report</title></head><body></body></html>";
        webBrowser.NavigateToString(blank);
    }

    private void buttonAnalyze_Click(object sender, RoutedEventArgs e)
    {
        var data = WindowsShortcutReader.OpenFile();

        string text = "<!doctype html><html style=\"background-color: #000000; color: #ffffff; font-family: sans-serif;\" lang=\"en\"><head><meta charset=\"utf-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"><title>Static Analysis Report</title></head><body>";

        if(data.Item1 == true)
        {
            Data = data.Item2;

            text += $"FileName: {WindowsShortcutReader.FileName}";

            text += $"<h2>ShellLinkHeader</h2>";
            text += $"HeaderSize: <span style=\"color: #0097ff;\">{ShellLinkHeader.HeaderSize.ValueAsText}</span><br />";
            if(ShellLinkHeader.HeaderSize.IsValid)
                text += $"HeaderSize.IsValid: <span style=\"color: #0097ff;\">True</span><br />";
            else
                text += $"HeaderSize.IsValid: <span style=\"color: #ff0040;\">False</span><br />";

            //to be continued.

            text += "</body></html>";

            webBrowser.NavigateToString(text);
        }
    }
}