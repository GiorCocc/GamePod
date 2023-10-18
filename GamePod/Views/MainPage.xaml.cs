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
        // TODO: get the selected item from the ListView
        var selectedContainer = ConstainerListView.SelectedItem;
        Debug.WriteLine("Selected container: " + selectedContainer);
        

        var dialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Title = "Run " + selectedContainer,
            Content = "Run docker container using Docker API service",
            CloseButtonText = "Ok"
        };

        await dialog.ShowAsync();

    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        Containers.Add("test 1");
        Containers.Add("test 2");
        Containers.Add("test 3");

        //Debug.WriteLine("Docker service is: " + App.GetService<IDockerService>().GetVersion());
        var service = App.GetService<IDockerService>();
        Debug.WriteLine("Docker service is: " + service);

        // wait for the service to complete the container list
        var containers = await service.ListContainers();
        foreach (var container in containers)
        {

            Debug.WriteLine("Container: " + container.Names[0]);
            Containers.Add(container.Names[0]);
        }


        // get the list of all the containers created using the Docker API service
        //DockerService service = (DockerService)App.GetService<IDockerService>();

        //// quando il servizio è creato, ottieni la lista di tutti i container creati
        //if (service != null)
        //{
        //       var containers = service.ListContainers().Result;
        //    foreach (var container in containers)
        //    {

        //        Containers.Add(container.Names[0]);
        //               }
        //} else
        //{
        
        //           Debug.WriteLine("Docker service is null");
        //       }

        
    }
}
