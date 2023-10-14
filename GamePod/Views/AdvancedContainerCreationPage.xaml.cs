using System.Diagnostics;
using System.Text.RegularExpressions;
using GamePod.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;

namespace GamePod.Views;

public sealed partial class AdvancedContainerCreationPage : Page
{
    public AdvancedContainerCreationViewModel ViewModel
    {
        get;
    }

    public AdvancedContainerCreationPage()
    {
        ViewModel = App.GetService<AdvancedContainerCreationViewModel>();
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
        {
            // add the text to the editor rich text box in order to be modified
            var command = FormatCommand((string)e.Parameter);
            editor.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, command);
        }

        base.OnNavigatedTo(e);
    }

    // TODO: separare ogni parametro del comando (ovvero gli elementi che iniziano con " --" o con " -") con un a capo e un segno di tabulazione
    private string FormatCommand(string parameter)
    {
        var command = parameter;

        // replace all the --<command> with a new line and a tab
        command = Regex.Replace(command, @"--", "\n\t--");

        return command;
    }

    private void DiscardButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        // go to the home page
        Frame.Navigate(typeof(HomePage));
    }

    private async void CreateContainerButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        // get all the text from the editor rich text box
        var command = string.Empty;
        editor.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out command);

        if (string.IsNullOrWhiteSpace(command))
        {
            // error dialog
            var error = new ContentDialog
            {

                Title = "Error",
                Content = "The command is empty",
                CloseButtonText = "Ok"
            };
        }
        else
        {
            // remove all the new lines
            command = Regex.Replace(command, @"\t|\n|\r", "");

            // create the dialog with a title, a message, the command that will be executed in the terminal
            var dialog = new ContentDialog
            {
                XamlRoot = XamlRoot,
                Title = "Create Container",
                Content = "Do you want to create the container?",
                PrimaryButtonText = "Create",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
            };

            // show 
            var result = await dialog.ShowAsync();
            Debug.WriteLine("Command: " + command);

            // if the developer clicks on the "Create" button
            if (result == ContentDialogResult.Primary)
            {
                ViewModel.CreateContainer(command);

                // TODO: go to the homepage
                Frame.Navigate(typeof(HomePage));
            }
        }
    }

    private void CopyButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        // get all the text from the editor rich text box
        var command = string.Empty;
        editor.Document.GetText(Microsoft.UI.Text.TextGetOptions.None, out command);

        // copy the command to the clipboard
        DataPackage dataPackage = new DataPackage();
        dataPackage.SetText(command);
        Clipboard.SetContent(dataPackage);

    }
}
