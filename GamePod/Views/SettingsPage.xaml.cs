using System.Diagnostics;
using GamePod.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace GamePod.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }

    //private void Update_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    //{
    //    // get the expander header
    //    var software = WingetExpander.Header.ToString().ToLower();
    //    Debug.WriteLine("Update Clicked for " + software);
    //    ViewModel.UpdateSoftware(software);
    //}
}
