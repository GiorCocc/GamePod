using GamePod.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage;
using AppUIBasics.Helper;
using System.Diagnostics;

namespace GamePod.Views;

public sealed partial class MainPage : Page
{
    // linux distributions for the ComboBox
    public List<string> LinuxDistros
    {
        get;
    } = new List<string> { "Ubuntu", "Fedora", "Debian", "Arch Linux" };

    public List<string> GameEngines
    {
        get;
    } = new List<string> { "Unity", "Godot", "Unreal Engine" };

    public HomeViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        InitializeComponent();
    }

    /*
     * This method will be called when the developer clicks on the "Pick Folder" button
     * It will open the explorer to allow the developer to choose the folder where the project exists
     */
    private async void PickFolderButton_Click(object sender, RoutedEventArgs e)
    {
        // create a folder picker
        FolderPicker folderPicker = new FolderPicker();
        folderPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
        folderPicker.FileTypeFilter.Add("*");

        // initialize the folder picker with the current WinUI3 window
        var window = WindowHelper.CreateWindow();
        var helper = WinRT.Interop.WindowNative.GetWindowHandle(window);
        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, helper);

        // open the folder picker
        StorageFolder folder = await folderPicker.PickSingleFolderAsync();

        if (folder != null)
        {
            StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
            PickFolderOutputTextBlock.Text = folder.Path;
        }
        
    }

    /*
     * This method will be called when the developer clicks on the "Create Container" button
     * It will create a dialog where the developer can see the command that will be executed in the terminal in order to create the container
     * The developer can choose to execute the command or not and, if he chooses to execute it, the container will be created
     * It can also choose (with a checkbox) to create the Dockerfile/Podmanfile and save it in the project folder
     */
    private async void CreateContainerButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        // get the properties of the project
        var projectName = ProjectNameTextBox.Text;
        var projectForlderPath = PickFolderOutputTextBlock.Text;
        var distributionIndex = DistributionComboBox.SelectedIndex;
        var gameEngineIndex = GameEngineComboBox.SelectedIndex;

        // check if one of the fields is empty; if so, show an error message
        if (projectName == "" || projectForlderPath == "" || distributionIndex < 0 || gameEngineIndex < 0)
        {
            var errorDialog = new ContentDialog
            {

                XamlRoot = XamlRoot,
                Title = "Error",
                Content = "Please fill all the fields",
                CloseButtonText = "Ok",
                DefaultButton = ContentDialogButton.Close,
            };

            await errorDialog.ShowAsync();
            return;
        }

        var distributionVersion = LinuxDistroVersionNumberTextBox.Text;
        if (distributionVersion == "")
        {
            distributionVersion = "latest";
        }

        var distribution = LinuxDistros[distributionIndex];
        var gameEngine = GameEngines[gameEngineIndex];

        // command to create the container
        var command = "Test command for " + projectName + " (" + projectForlderPath + ") with " + distribution + " " + distributionVersion + " and " + gameEngine + " game engine";

        // create the dialog with a title, a message, the command that will be executed in the terminal
        var dialog = new ContentDialog
        {
            XamlRoot = XamlRoot,
            Title = "Create Container",
            Content = "Do you want to create the container?\nCommand: " + command,
            PrimaryButtonText = "Create",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
        };

        // show 
        var result = await dialog.ShowAsync();

        // if the developer clicks on the "Create" button
        if (result == ContentDialogResult.Primary)
        {
            ViewModel.CreateContainer(command);

            // go to the homepage
            Frame.Navigate(typeof(HomePage));
        }

    }

    private void DiscardButton_Click(object sender, RoutedEventArgs e)
    {
        // go to the homepage
        Frame.Navigate(typeof(HomePage));
    }
}
