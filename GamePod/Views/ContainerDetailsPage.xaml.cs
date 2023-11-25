using System.Diagnostics;
using GamePod.Contracts.Services;
using GamePod.Models;
using GamePod.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

namespace GamePod.Views;

public sealed partial class ContainerDetailsPage : Page
{
    private Container container
    {
        get; set;
    }

    public ContainerDetailsViewModel ViewModel
    {
        get;
    }

    private string ProjectName { get; set; } = string.Empty;
    private string ContainerState { get; set; } = string.Empty;
    private string ContainerImage { get; set; } = string.Empty;
    private string ContainerID { get; set; } = string.Empty;
    private string ContainerPlatform { get; set; } = string.Empty;
    private string LastUsage { get; set; } = string.Empty;
    private string ContainerCommand { get; set; } = string.Empty;
    private string ContainerInspect { get; set; } = string.Empty;
    private string ProjectFolderHost { get; set; } = string.Empty;
    private string Binds { get; set; } = string.Empty;
    private string Envs { get; set; } = string.Empty;
    private string Ports { get; set; } = string.Empty;

    private IDockerService service;

    public ContainerDetailsPage()
    {
        ViewModel = App.GetService<ContainerDetailsViewModel>();
        InitializeComponent();

        service = App.GetService<IDockerService>();
    }

    #region Navigation
    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        Debug.WriteLine($"ContainerDetailsPage.OnNavigatedTo: {e.Parameter}");

        // Get the container from the parameter
        container = e.Parameter as Container;

        // Set the container name in the title
        ProjectName = container.ContainerName;
        ContainerImage = container.ContainerImage;
        ContainerID = container.ContainerID;
        ContainerInspect = JsonConvert.SerializeObject(container.InspectResponse, Formatting.Indented);
        ContainerState = "(" + container.ContainerStatus + " ~ " + container.ContainerExitCode + ")";
        ContainerPlatform = container.ContainerPlatform;
        LastUsage = container.ContainerFinishedAt;
        ContainerCommand = container.ContainerCommand;
        ProjectFolderHost = container.InspectResponse.HostConfig.Binds[0];
        // get all the binds
        foreach (var bind in container.InspectResponse.HostConfig.Binds)
        {
            Binds += bind + "\n";
        }
        // get all the envs
        foreach (var env in container.InspectResponse.Config.Env)
        {

            Envs += env + "\n";
        }
        // get all the ports
        foreach (var port in container.InspectResponse.HostConfig.PortBindings)
        {


            Ports += port.Key + " -> " + port.Value[0].HostPort + "\n";
        }

        LoadMarkdownTextBlock();

    }

    #endregion

    private async void GetContainerLogs()
    {
        await service.GetContainerLogs(container.InspectResponse.ID);
    }

    private async void LoadMarkdownTextBlock()
    {
        // controlla se non è presente il file Markdown nella definizione del game engine
        if (container.ContainerGuidePath == "")
        {
            GuideMarkdownTextBlock.Text = "> Pod " + container.ContainerName + " was not created with GamePod. No guide available.";
            return;
        }

        // Sostituisci "NomeFileMarkdown.md" con il percorso del tuo file Markdown
        StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(container.ContainerGuidePath));

        // Leggi il contenuto del file
        string markdownContent = await FileIO.ReadTextAsync(file);

        // Assegna il contenuto al controllo MarkdownTextBlock
        GuideMarkdownTextBlock.Text = markdownContent;
    }

    #region Container management Buttons

    private async void PlayButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ProgressDialog progressDialog = new ProgressDialog();
        progressDialog.XamlRoot = this.Content.XamlRoot;
        var progressResult = progressDialog.ShowAsync();

        try
        {
            await ViewModel.StartContainer(container.ContainerName);
            progressDialog.Hide();
            UpdateContainerInformationFromInspect();
        }
        catch (Exception ex)
        {

            Debug.WriteLine("Exception: " + ex.Message);
        }

    }

    private async void StopButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var containerName = container.ContainerName;

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
            Frame.Navigate(typeof(HomePage));
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Exception: " + ex.Message);
        }

        // TODO: implementare il metodo per aggiornare la lista dei container
    }

    private async void RestartButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var containerName = container.ContainerName;
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
            UpdateContainerInformationFromInspect();
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Exception: " + ex.Message);
        }
    }

    private async void DeleteButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var containerName = container.ContainerName;
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
            Frame.Navigate(typeof(HomePage));

        }
        catch (Exception ex)
        {
            Debug.WriteLine("Exception: " + ex.Message);
        }
    }

    private async void PauseButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var containerName = container.ContainerName;

        ProgressDialog progressDialog = new ProgressDialog();
        progressDialog.XamlRoot = this.Content.XamlRoot;
        var progressResult = progressDialog.ShowAsync();
        try
        {
            await ViewModel.PauseContainer(containerName);
            progressDialog.Hide();
            UpdateContainerInformationFromInspect();
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Exception: " + ex.Message);
        }

    }

    private void ExecButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var containerName = container.ContainerName;

        // open the windows terminal
        Process process = new Process();
        process.StartInfo.FileName = "wt.exe";
        process.StartInfo.Arguments = "docker exec -it " + containerName + " " + ContainerCommand;
        process.Start();

    }

    #endregion

    private async void UpdateContainerInformationFromInspect()
    {
        await container.GetInspectResponse(container);
        container.CompleteInformationFromInspect(container.InspectResponse);

        ContainerInspect = JsonConvert.SerializeObject(container.InspectResponse, Formatting.Indented);
        ContainerState = "(" + container.ContainerStatus + " ~ " + container.ContainerExitCode + ")";

        // update the UI
        ContainerDetailsRichTextBlock.Blocks.Clear();
        ContainerDetailsRichTextBlock.Blocks.Add(new Paragraph { Inlines = { new Run { Text = ContainerInspect } } });
        ContainerStateTextBlock.Text = ContainerState;
        StatusTextBlock.Text = ContainerState;
        LastUsageTextBlock.Text = container.ContainerFinishedAt;
    }

    private void CopyButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var id = ContainerID;

        // copy the command to the clipboard
        DataPackage dataPackage = new DataPackage();
        dataPackage.SetText(id);
        Clipboard.SetContent(dataPackage);

        // change the icon of the button to a check
        Icon.SetValue(SymbolIcon.SymbolProperty, Symbol.Accept);
    }

    private void ProjectFolderHostButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var path = ProjectFolderHostTextBlock.Text;
        path = path.Remove(path.Length - 9);
        
        // open the file explorer to the project folder
        Process process = new Process();
        process.StartInfo.FileName = "explorer.exe";
        process.StartInfo.Arguments = path;
        process.Start();
    }
}
