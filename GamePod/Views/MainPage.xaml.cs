using System.Collections.ObjectModel;
using System.Diagnostics;
using GamePod.Contracts.Services;
using GamePod.Models;
using GamePod.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Navigation;

namespace GamePod.Views;

public sealed partial class HomePage : Page
{
    public ObservableCollection<ContainerObject> _containers = new ObservableCollection<ContainerObject>();
    public ObservableCollection<ContainerObject> Containers => this._containers;

    public MainViewModel ViewModel
    {
        get;
    }

    public HomePage()
    {
        ViewModel = App.GetService<MainViewModel>();

        InitializeComponent();
    }

    #region Top Buttons
    private void CreateButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        // go to the CreatePage
        Frame.Navigate(typeof(MainPage));
    }
    #endregion


    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        //Debug.WriteLine("Docker service is: " + App.GetService<IDockerService>().GetVersion());
        var service = App.GetService<IDockerService>();
        //Debug.WriteLine("Docker service is: " + service);

        // wait for the service to complete the container list
        var containers = await service.ListContainers();
        foreach (var container in containers)
        {
            Debug.WriteLine("Container: " + container.Names[0]);
            Containers.Add(new ContainerObject(container.Names[0], container.State));
        }

    }

    #region Container management Buttons
    private void PlayButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedContainer = ConstainerListView.SelectedItem;
        Debug.WriteLine("Selected container: " + selectedContainer);

        // get the container object
        if (sender is AppBarButton)
        {
            selectedContainer = ConstainerListView.SelectedItem;
        }
        else if (sender is Button)
        {
            selectedContainer = (sender as Button).DataContext;
        }
        else if (sender is MenuFlyoutItem)
        {
            selectedContainer = (sender as MenuFlyoutItem).DataContext;
        }
        else
        {
            Debug.WriteLine("Sender: " + sender.GetType());
            return;
        }

        // get the container object
        var containerName = selectedContainer.GetType().GetProperty("Name").GetValue(selectedContainer, null);
        Debug.WriteLine("Container name: " + containerName);

        ViewModel.StartContainer(containerName);

    }

    private async void StopButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedContainer = ConstainerListView.SelectedItem;

        // get the container object
        if (sender is AppBarButton)
        {
            selectedContainer = ConstainerListView.SelectedItem;
        }
        else if (sender is Button)
        {
            selectedContainer = (sender as Button).DataContext;
        }
        else if (sender is MenuFlyoutItem)
        {
            selectedContainer = (sender as MenuFlyoutItem).DataContext;
        }
        else
        {
            Debug.WriteLine("Sender: " + sender.GetType());
            return;
        }

        var containerName = selectedContainer.GetType().GetProperty("Name").GetValue(selectedContainer, null);
        Debug.WriteLine("Selected container: " + containerName);

        var dialog = new ContentDialog
        {
            XamlRoot = this.Content.XamlRoot,
            Title = "Stop container",
            Content = "Are you sure you want to stop the container " + containerName + "?",
            PrimaryButtonText = "Stop",
            CloseButtonText = "Cancel"
        };
        var result = await dialog.ShowAsync();

        if (result != ContentDialogResult.Primary)
        {
            return;
        }

        ViewModel.StopContainer(containerName);

        // TODO: implementare il metodo per aggiornare la lista dei container
    }

    private async void RestartButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedContainer = ConstainerListView.SelectedItem;

        // get the container object
        if (sender is AppBarButton)
        {
            selectedContainer = ConstainerListView.SelectedItem;
        }
        else if (sender is Button)
        {
            selectedContainer = (sender as Button).DataContext;
        }
        else if (sender is MenuFlyoutItem)
        {
            selectedContainer = (sender as MenuFlyoutItem).DataContext;
        }
        else
        {
            Debug.WriteLine("Sender: " + sender.GetType());
            return;
        }

        var containerName = selectedContainer.GetType().GetProperty("Name").GetValue(selectedContainer, null);
        Debug.WriteLine("Selected container: " + containerName);

        var dialog = new ContentDialog
        {
            XamlRoot = this.Content.XamlRoot,
            Title = "Restart container",
            Content = "Are you sure you want to restart the container " + containerName + "?",
            PrimaryButtonText = "Restart",
            CloseButtonText = "Cancel"
        };

        var result = await dialog.ShowAsync();

        if (result != ContentDialogResult.Primary)
        {

            return;
        }

        ViewModel.RestartContainer(containerName);
    }

    private async void DeleteButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedContainer = ConstainerListView.SelectedItem;

        // get the container object
        if (sender is AppBarButton)
        {
            selectedContainer = ConstainerListView.SelectedItem;
        }
        else if (sender is Button)
        {
            selectedContainer = (sender as Button).DataContext;
        }
        else if (sender is MenuFlyoutItem)
        {
            selectedContainer = (sender as MenuFlyoutItem).DataContext;
        }
        else
        {
            Debug.WriteLine("Sender: " + sender.GetType());
            return;
        }

        var containerName = selectedContainer.GetType().GetProperty("Name").GetValue(selectedContainer, null);
        Debug.WriteLine("Selected container: " + containerName);

        var dialog = new ContentDialog
        {
            XamlRoot = this.Content.XamlRoot,
            Title = "Delete container",
            Content = "Are you sure you want to delete the container " + containerName + "?",
            PrimaryButtonText = "Delete",
            CloseButtonText = "Cancel"
        };
        var result = await dialog.ShowAsync();

        if (result != ContentDialogResult.Primary)
        {
            return;
        }

        ViewModel.DeleteContainer(containerName);

        // TODO: implementare il metodo per aggiornare la lista dei container 
    }

    private void PauseButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedContainer = ConstainerListView.SelectedItem;

        // get the container object
        if (sender is AppBarButton)
        {
            selectedContainer = ConstainerListView.SelectedItem;
        }
        else if (sender is Button)
        {
            selectedContainer = (sender as Button).DataContext;
        }
        else if (sender is MenuFlyoutItem)
        {
            selectedContainer = (sender as MenuFlyoutItem).DataContext;
        }
        else
        {
            Debug.WriteLine("Sender: " + sender.GetType());
            return;
        }

        var containerName = selectedContainer.GetType().GetProperty("Name").GetValue(selectedContainer, null);

        ViewModel.PauseContainer(containerName);

    }

    #endregion

    private void TerminalButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Debug.WriteLine("Terminal button pressed");

        Process process = new Process();
        process.StartInfo.FileName = "wt.exe";

        process.Start();
    }

    private void DockerDesktopButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Debug.WriteLine("Docker Desktop button pressed");

        Process process = new Process();
        process.StartInfo.FileName = "C:\\Program Files\\Docker\\Docker\\Docker Desktop.exe";
        process.Start();
    }

    private void ListRightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
    {
        FlyoutBase.ShowAttachedFlyout((Microsoft.UI.Xaml.FrameworkElement)sender);
    }

    // TODO: implementare il metodo per aggiornare la lista dei container quando viene premuto uno qualsiasi dei bottoni (Stop, Delete)
    //private void UpdateContainerList()
    //{
    //    // clear the list
    //    Containers.Clear();

    //    // update the list of containers
    //    var service = App.GetService<IDockerService>();
    //    var containers = service.ListContainers().Result;
    //    foreach (var container in containers)
    //    {

    //        Debug.WriteLine("Container: " + container.Names[0]);
    //        Containers.Add(container.Names[0]);
    //    }
    //}
}
