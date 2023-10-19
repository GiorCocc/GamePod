using System.Collections.ObjectModel;
using System.Diagnostics;
using Docker.DotNet;
using Docker.DotNet.Models;
using GamePod.Contracts.Services;
using GamePod.Models;
using GamePod.Services;
using GamePod.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace GamePod.Views;

public sealed partial class HomePage : Page
{
    public ObservableCollection<string> _containers = new ObservableCollection<string>();
    public ObservableCollection<string> Containers {
        get
        {
            return this._containers;
        } 
    }

    public MainViewModel ViewModel { get; }
    
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

    private async void PlayAppBarButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedContainer = ConstainerListView.SelectedItem;
        Debug.WriteLine("Selected container: " + selectedContainer);
        
        ViewModel.StartContainer(selectedContainer);

    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        Containers.Add("test 1");
        Containers.Add("test 2");
        Containers.Add("test 3");

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

    private void StopAppBarButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedContainer = ConstainerListView.SelectedItem;
        Debug.WriteLine("Selected container: " + selectedContainer);
        ViewModel.StopContainer(selectedContainer);

    }

    private void RestartAppBarButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedContainer = ConstainerListView.SelectedItem;
        Debug.WriteLine("Selected container: " + selectedContainer);
        ViewModel.RestartContainer(selectedContainer);

    }

    private void DeleteAppBarButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedContainer = ConstainerListView.SelectedItem;
        Debug.WriteLine("Selected container: " + selectedContainer);
        ViewModel.DeleteContainer(selectedContainer);

    }

    private void PauseAppBarButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedContainer = ConstainerListView.SelectedItem;
        Debug.WriteLine("Selected container: " + selectedContainer);
        ViewModel.PauseContainer(selectedContainer);

    }
}
