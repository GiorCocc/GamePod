﻿using System.Diagnostics;
using AppUIBasics.Helper;
using Docker.DotNet.Models;
using GamePod.Models;
using GamePod.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;

namespace GamePod.Views;

public sealed partial class MainPage : Page
{
    // linux distributions for the ComboBox
    public List<string> LinuxDistros { get; }
    public List<string> GameEngines { get; }
    public HomeViewModel ViewModel { get; }

    public MainPage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        LinuxDistros = LinuxDistribution.GetLinuxDistroNamesList();
        GameEngines = GameEngine.GetGameEngineNamesList();

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
            // print the forlder path to the correct TextBox
            if (sender == PickFolderButton)
            {
                PickFolderOutputTextBlock.Text = folder.Path;
            }
            else if (sender == PickFolderButton2)
            {
                PickFolderOutputTextBlock2.Text = folder.Path;
            }
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
        var gameEngineIndex = GameEngineComboBox.SelectedIndex;

        // check if one of the fields is empty; if so, show an error message
        if (projectName == "" || projectForlderPath == "" || gameEngineIndex < 0)
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

        var gameEngine = GameEngines[gameEngineIndex];
        var gameEngineVersion = GameEngineVersionTextBox.Text;
        var destroyAfterUse = Convert.ToBoolean(DestroyAfterUseToggleSwitch.GetValue(ToggleSwitch.IsOnProperty));
        var port = "";
        var cpuCores = 0;
        var ram = "";
        var gpu = "";
        var otherFolder = PickFolderOutputTextBlock2.Text;
        var destinationPath = DestinationPathTextBox.Text;

        // get the value inside the textbox only if the toggle switch is on
        if (Convert.ToBoolean(AddPortToggleSwitch.GetValue(ToggleSwitch.IsOnProperty)))
        {
            port = PortTextBox.Text;
        }

        if (Convert.ToBoolean(PerformanceToggleSwitch.GetValue(ToggleSwitch.IsOnProperty)))
        {
            cpuCores = CPUComboBox.SelectionBoxItem == null ? 0 : (Convert.ToInt32(CPUComboBox.SelectionBoxItem.ToString()) * 1000000000);
            ram = RAMComboBox.SelectionBoxItem == null ? "" : RAMComboBox.SelectionBoxItem.ToString().Substring(0, RAMComboBox.SelectionBoxItem.ToString().Length - 3) + "g";
            
        }


        // create a container object
        var container = new ContainerParameters(projectName, projectForlderPath, gameEngine, gameEngineVersion, destroyAfterUse, port, cpuCores, ram, otherFolder, destinationPath);

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

        // if the developer clicks on the "Create" button
        if (result != ContentDialogResult.Primary)
        {
            return;
        }

        ProgressDialog progressDialog = new ProgressDialog();
        progressDialog.XamlRoot = this.Content.XamlRoot;
        var progress = progressDialog.ShowAsync();

        try
        {
            await ViewModel.CreateContainer(container.ContainerParam);
            progressDialog.Hide();
            Frame.Navigate(typeof(HomePage));
        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.Message);
            progressDialog.Hide();
            var errorDialog = new ContentDialog
            {
                XamlRoot = XamlRoot,
                Title = "Error",
                Content = ex.Message,
                CloseButtonText = "Ok",
                DefaultButton = ContentDialogButton.Close,
            };

            await errorDialog.ShowAsync();
        }
    }

    private void DiscardButton_Click(object sender, RoutedEventArgs e)
    {
        // go to the homepage
        Frame.Navigate(typeof(HomePage));
    }

    private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
    {
        if (sender == AddPortToggleSwitch)
        {
            if (Convert.ToBoolean(AddPortToggleSwitch.GetValue(ToggleSwitch.IsOnProperty)))
            {
                PortNumberSettingsCard.IsEnabled = true;

            }
            else
            {

                PortNumberSettingsCard.IsEnabled = false;
            }
        }
        else if (sender == PerformanceToggleSwitch)
        {
            if (Convert.ToBoolean(PerformanceToggleSwitch.GetValue(ToggleSwitch.IsOnProperty)))
            {
                CPUSettingsCard.IsEnabled = true;
                RAMSettingsCard.IsEnabled = true;
            }
            else
            {
                CPUSettingsCard.IsEnabled = false;
                RAMSettingsCard.IsEnabled = false;
            }
        }
    }

    // FIXME
    private async void AdvancedContainerSettingsButton_Click(object sender, RoutedEventArgs e)
    {
        // get the properties of the project
        var projectName = ProjectNameTextBox.Text;
        var projectForlderPath = PickFolderOutputTextBlock.Text;
        var gameEngineIndex = GameEngineComboBox.SelectedIndex;

        // check if one of the fields is empty; if so, show an error message
        if (projectName == "" || projectForlderPath == "" || gameEngineIndex < 0)
        {
            var defaultRunCommand = "docker run -it --rm ";
            Frame.Navigate(typeof(AdvancedContainerCreationPage), defaultRunCommand);
        }
        else
        {
            var gameEngine = GameEngines[gameEngineIndex];
            var gameEngineVersion = GameEngineVersionTextBox.Text;
            var destroyAfterUse = Convert.ToBoolean(DestroyAfterUseToggleSwitch.GetValue(ToggleSwitch.IsOnProperty));
            var port = "";
            var cpuCores = 0;
            var ram = "";
            var gpu = "";
            var otherFolder = PickFolderOutputTextBlock2.Text;
            var destinationPath = DestinationPathTextBox.Text;

            if (Convert.ToBoolean(PerformanceToggleSwitch.GetValue(ToggleSwitch.IsOnProperty)))
            {
                port = PortTextBox.Text;
            }

            if (Convert.ToBoolean(PerformanceToggleSwitch.GetValue(ToggleSwitch.IsOnProperty)))
            {
                cpuCores = 0;
                ram = RAMComboBox.SelectionBoxItem.ToString().Substring(0, RAMComboBox.SelectionBoxItem.ToString().Length - 3);
                
            }


            // create a container object
            var container = new ContainerParameters(projectName, projectForlderPath, gameEngine, gameEngineVersion, destroyAfterUse, port, cpuCores, ram, otherFolder, destinationPath);

            // Go to AdvancedContainerCreationPage
            //Frame.Navigate(typeof(AdvancedContainerCreationPage), command);
        }


    }
}
