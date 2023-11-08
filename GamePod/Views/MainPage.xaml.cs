using System.Collections.ObjectModel;
using System.Diagnostics;
using GamePod.Contracts.Services;
using GamePod.Models;
using GamePod.ViewModels;

using Microsoft.UI.Xaml.Controls;
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

    #endregion



    #region Container management Buttons

    private async void PlayButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
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

        ProgressDialog progressDialog = new ProgressDialog();
        progressDialog.XamlRoot = this.Content.XamlRoot;
        var progressResult = progressDialog.ShowAsync();

        try
        {
            await ViewModel.StartContainer(containerName);
            progressDialog.Hide();
            var message = "Container " + containerName + " started successfully";
            AddInfoBarItem(message, InfoBarSeverity.Success);
            UpdateContainerList();
        }
        catch (Exception ex)
        {

            Debug.WriteLine("Exception: " + ex.Message);
            AddInfoBarItem(ex.Message, InfoBarSeverity.Error);
        }

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

        ProgressDialog progressDialog = new ProgressDialog();
        progressDialog.XamlRoot = this.Content.XamlRoot;
        var progressResult = progressDialog.ShowAsync();

        try
        {
            await ViewModel.StopContainer(containerName);
            progressDialog.Hide();
            var message = "Container " + containerName + " stopped successfully";
            AddInfoBarItem(message, InfoBarSeverity.Success);
            UpdateContainerList();
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Exception: " + ex.Message);
            AddInfoBarItem(ex.Message, InfoBarSeverity.Error);
        }

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

        ProgressDialog progressDialog = new ProgressDialog();
        progressDialog.XamlRoot = this.Content.XamlRoot;
        var progressResult = progressDialog.ShowAsync();
        try
        {
            await ViewModel.RestartContainer(containerName);
            progressDialog.Hide();
            var message = "Container " + containerName + " restarted successfully";
            AddInfoBarItem(message, InfoBarSeverity.Success);
            UpdateContainerList();
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Exception: " + ex.Message);
            AddInfoBarItem(ex.Message, InfoBarSeverity.Error);
        }
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

        ProgressDialog progressDialog = new ProgressDialog();
        progressDialog.XamlRoot = this.Content.XamlRoot;
        var progressResult = progressDialog.ShowAsync();
        try
        {
            await ViewModel.DeleteContainer(containerName);

            progressDialog.Hide();

            var message = "Container " + containerName + " deleted successfully";

            AddInfoBarItem(message, InfoBarSeverity.Success);

            UpdateContainerList();

        }
        catch (Exception ex)
        {
            Debug.WriteLine("Exception: " + ex.Message);
            AddInfoBarItem(ex.Message, InfoBarSeverity.Error);
        }
    }

    private async void PauseButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
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

        ProgressDialog progressDialog = new ProgressDialog();
        progressDialog.XamlRoot = this.Content.XamlRoot;
        var progressResult = progressDialog.ShowAsync();
        try
        {
            await ViewModel.PauseContainer(containerName);
            progressDialog.Hide();
            var message = "Container " + containerName + " paused successfully";
            AddInfoBarItem(message, InfoBarSeverity.Success);
            UpdateContainerList();
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Exception: " + ex.Message);
            AddInfoBarItem(ex.Message, InfoBarSeverity.Error);
        }

    }

    private void ExecButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedContainer = (sender as MenuFlyoutItem).DataContext;
        var containerName = selectedContainer.GetType().GetProperty("Name").GetValue(selectedContainer, null);

        // open the windows terminal
        Process process = new Process();
        process.StartInfo.FileName = "wt.exe";
        process.StartInfo.Arguments = "docker exec -it " + containerName + " bash";
        process.Start();

    }

    // TODO: creare una pagina dove mostrare i dati del container creato
    private async void ContainerInfoButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var selectedContainer = (sender as MenuFlyoutItem).DataContext;
        var containerName = selectedContainer.GetType().GetProperty("Name").GetValue(selectedContainer, null);
        Container container = new Container(containerName.ToString());
        await container.GetInspectResponse(container);

        // Go to Details page
        Frame.Navigate(typeof(ContainerDetailsPage), container);

        //var dialog = new ContentDialog
        //{
        //    XamlRoot = this.Content.XamlRoot,
        //    Title = "Container info",
        //    Content = "Container info for " + containerName + " to be displayed",
        //    PrimaryButtonText = "OK",
        //    CloseButtonText = "Cancel"
        //};
        //var result = await dialog.ShowAsync();
    }

    #endregion

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        UpdateContainerList();

    }

    private void AddInfoBarItem(string message, InfoBarSeverity severity)
    {

        InfoBarContainer.Children.Clear();

        InfoBar infoBar = new InfoBar
        {

            XamlRoot = this.Content.XamlRoot,
            Message = message,
            Severity = severity,
            IsOpen = true,
            IsClosable = true
        };


        infoBar.CloseButtonClick += (sender, args) => infoBar.IsOpen = false;

        InfoBarContainer.Children.Add(infoBar);

    }

    private async void UpdateContainerList()
    {
        // clear the list
        Containers.Clear();

        // update the list of containers
        var service = App.GetService<IDockerService>();
        var containers = await service.ListContainers();

        foreach (var container in containers)
        {
            Debug.WriteLine("Container: " + container.Names[0]);
            Containers.Add(new ContainerObject(container.Names[0], container.State));
        }
    }

    private void ReloadContainerListButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        UpdateContainerList();
    }

    
}
