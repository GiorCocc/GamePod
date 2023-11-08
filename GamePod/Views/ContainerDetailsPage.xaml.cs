using System.Diagnostics;
using Docker.DotNet.Models;
using GamePod.Models;
using GamePod.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.Security.AccessControl;
using Newtonsoft.Json;

namespace GamePod.Views;

public sealed partial class ContainerDetailsPage : Page
{
    private Container container { get; set; }

    public ContainerDetailsViewModel ViewModel { get; }
    private string ProjectName { get; set; } = string.Empty;
    private string ContainerImage { get; set; } = string.Empty;
    private string ContainerID { get; set; } = string.Empty;
    private string ContainerInspect { get; set; } = string.Empty;

    public ContainerDetailsPage()
    {
        ViewModel = App.GetService<ContainerDetailsViewModel>();
        InitializeComponent();
    }

    #region Navigation
    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        Debug.WriteLine($"ContainerDetailsPage.OnNavigatedTo: {e.Parameter}");

        // Get the container from the parameter
        container = e.Parameter as Container;

        // Set the container name in the title
        ProjectName = container.inspectResponse.Name;
        ContainerImage = container.inspectResponse.Config.Image;
        ContainerID = container.inspectResponse.Config.Hostname;
        ContainerInspect = JsonConvert.SerializeObject(container.inspectResponse, Formatting.Indented);

    }
    #endregion
}
