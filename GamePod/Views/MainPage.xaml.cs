using System.Collections.ObjectModel;
using System.Diagnostics;
using GamePod.Contracts.Services;
using GamePod.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace GamePod.Views;

public sealed partial class HomePage : Page
{
    public ObservableCollection<string> _containers = new ObservableCollection<string>();
    public ObservableCollection<string> Containers
    {
        get
        {
            return this._containers;
        }
    }

    public MainViewModel ViewModel
    {
        get;
    }

    public HomePage()
    {
        ViewModel = App.GetService<MainViewModel>();

        InitializeComponent();
    }

    private void CreateButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        // go to the CreatePage
        Frame.Navigate(typeof(MainPage));
    }

    private void PlayAppBarButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedContainer = ConstainerListView.SelectedItem;
        Debug.WriteLine("Selected container: " + selectedContainer);

        ViewModel.StartContainer(selectedContainer);

    }

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
            Containers.Add(container.Names[0]);
        }


    }

    private async void StopAppBarButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedContainer = ConstainerListView.SelectedItem;
        Debug.WriteLine("Selected container: " + selectedContainer);

        var dialog = new ContentDialog
        {
            XamlRoot = this.Content.XamlRoot,
            Title = "Stop container",
            Content = "Are you sure you want to stop the container " + selectedContainer + "?",
            PrimaryButtonText = "Stop",
            CloseButtonText = "Cancel"
        };
        var result = await dialog.ShowAsync();

        if (result != ContentDialogResult.Primary)
        {
            return;
        }

        ViewModel.StopContainer(selectedContainer);

        // TODO: implementare il metodo per aggiornare la lista dei container
    }

    private async void RestartAppBarButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedContainer = ConstainerListView.SelectedItem;
        Debug.WriteLine("Selected container: " + selectedContainer);

        var dialog = new ContentDialog
        {
            XamlRoot = this.Content.XamlRoot,
            Title = "Restart container",
            Content = "Are you sure you want to restart the container " + selectedContainer + "?",
            PrimaryButtonText = "Restart",
            CloseButtonText = "Cancel"
        };
        var result = await dialog.ShowAsync();

        if (result != ContentDialogResult.Primary)
        {

            return;
        }

        ViewModel.RestartContainer(selectedContainer);
    }

    private async void DeleteAppBarButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedContainer = ConstainerListView.SelectedItem;
        Debug.WriteLine("Selected container: " + selectedContainer);

        var dialog = new ContentDialog
        {
            XamlRoot = this.Content.XamlRoot,
            Title = "Delete container",
            Content = "Are you sure you want to delete the container " + selectedContainer + "?",
            PrimaryButtonText = "Delete",
            CloseButtonText = "Cancel"
        };
        var result = await dialog.ShowAsync();

        if (result != ContentDialogResult.Primary)
        {


            return;
        }

        ViewModel.DeleteContainer(selectedContainer);

        // TODO: implementare il metodo per aggiornare la lista dei container 
    }

    private void PauseAppBarButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedContainer = ConstainerListView.SelectedItem;
        Debug.WriteLine("Selected container: " + selectedContainer);
        ViewModel.PauseContainer(selectedContainer);

    }

    // TODO: implementare il metodo per aggiornare la lista dei container quando viene premuto uno qualsiasi dei bottoni (Stop, Delete)
    private void UpdateContainerList()
    {
        // clear the list
        Containers.Clear();

        // update the list of containers
        var service = App.GetService<IDockerService>();
        var containers = service.ListContainers().Result;
        foreach (var container in containers)
        {

            Debug.WriteLine("Container: " + container.Names[0]);
            Containers.Add(container.Names[0]);
        }
    }
}
