/*
 * GamePod
 * 
 * Created by Giorgio Coccapani
 * Date: 20/09/2023
 * 
 * View Model for the Settings Page
 * In this page the developer can change the theme of the app (light, dark, system), the language of the app (English, Italian, etc.)
 * and choose the container engine (Docker or Podman) to use.
 * Given the container engine, the app will use the correct commands to start, stop, delete, etc. the container. 
 * It also checks if the container engine is installed and, if not, it will install it via winget (installing winget if needed).
 * 
 */

using System.Reflection;
using System.Windows.Input;
using System.Diagnostics;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using GamePod.Contracts.Services;
using GamePod.Helpers;

using Microsoft.UI.Xaml;

using Windows.ApplicationModel;

namespace GamePod.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;

    [ObservableProperty]
    private ElementTheme _elementTheme;

    [ObservableProperty]
    private string _versionDescription;

    [ObservableProperty]
    private string _wingetVersionNumber;
    [ObservableProperty]
    private string _dockerVersionNumber;
    [ObservableProperty]
    private string _wslVersionNumber;

    public ICommand SwitchThemeCommand
    {
        get;
    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();
        _wingetVersionNumber = GetVersionNumber("winget");
        _dockerVersionNumber = GetVersionNumber("docker");
        _wslVersionNumber = GetVersionNumber("wsl");

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });
    }

    // FIXME: this is a workaround to get the version number of the software
    //        it should be replaced with a better solution that doesn't open a terminal window every time the user opens the settings page
    //       (maybe a background task that runs at startup and saves the version number in a file?)
    private string GetVersionNumber(string software)
    {
        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo(software, "--version");
        startInfo.RedirectStandardOutput = true;
        startInfo.UseShellExecute = false; // this changes the parent window to a background window
        process.StartInfo = startInfo;
        process.Start();

        var output = process.StandardOutput.ReadToEnd();

        output = output.Replace("\n", "");

        return output;
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build} ({version.Revision})";
    }

    public void UpdateSoftware(string software)
    {
        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo(software, "upgrade");

        // display the terminal window to show the progress of the update
        startInfo.UseShellExecute = true;

        process.StartInfo = startInfo;
        process.Start();
    }
}
