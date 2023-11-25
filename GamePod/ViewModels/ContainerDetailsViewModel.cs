using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using GamePod.Contracts.Services;
using GamePod.Models;

namespace GamePod.ViewModels;

public partial class ContainerDetailsViewModel : ObservableRecipient
{
    private IDockerService service;

    public ContainerDetailsViewModel()
    {
        service = App.GetService<IDockerService>();
    }

    internal async Task<Container> GetContainer(string? containerName)
    {
        Container container = new Container(containerName);
        await container.GetInspectResponse(container);

        return container;
    }

    internal async Task DeleteContainer(object selectedContainer)
    {
        if (selectedContainer is not string)
        {
            Debug.WriteLine("Selected container is not a string: " + selectedContainer);
            return;
        }


        try
        {
            await service.DeleteContainer(selectedContainer as string);
        }
        catch (Exception e)
        {

            Debug.WriteLine("Exception: " + e.Message);
        }
    }

    internal async Task PauseContainer(object selectedContainer)
    {
        if (selectedContainer is not string)
        {
            Debug.WriteLine("Selected container is not a string: " + selectedContainer);
            return;
        }

        try
        {

            await service.PauseContainer(selectedContainer as string);
        }
        catch (Exception e)
        {


            Debug.WriteLine("Exception: " + e.Message);
        }
    }

    internal async Task RestartContainer(object selectedContainer)
    {
        if (selectedContainer is not string)
        {
            Debug.WriteLine("Selected container is not a string: " + selectedContainer);
            return;
        }

        try
        {


            await service.RestartContainer(selectedContainer as string);
        }
        catch (Exception e)
        {
            Debug.WriteLine("Exception: " + e.Message);
        }
    }

    internal async Task StartContainer(object selectedContainer)
    {
        if (selectedContainer is not string)
        {
            Debug.WriteLine("Selected container is not a string: " + selectedContainer);
        }

        try
        {
            await service.StartContainer(selectedContainer as string);
        }
        catch (Exception e)
        {
            Debug.WriteLine("Exception: " + e.Message);
        }

        // open a terminal to the container
        Process process = new Process();
        process.StartInfo.FileName = "wt.exe";
        process.StartInfo.Arguments = "docker exec -it " + selectedContainer + " bash";
        process.Start();

    }

    internal async Task StopContainer(object selectedContainer)
    {
        if (selectedContainer is not string)
        {
            Debug.WriteLine("Selected container is not a string: " + selectedContainer);
            return;
        }

        try
        {
            await service.StopContainer(selectedContainer as string);
        }
        catch (Exception e)
        {

            Debug.WriteLine("Exception: " + e.Message);
        }
    }

    internal async Task ExportContainer(string containerName)
    {
        if (containerName is not string)
        {

            Debug.WriteLine("Selected container is not a string: " + containerName);
            return;
        }

        var container = await service.GetContainerInspect(containerName);
        var projectPath = container.HostConfig.Binds[0];
        projectPath = projectPath.Substring(0, projectPath.LastIndexOf(':'));

        if (!Directory.Exists(projectPath + "/docker"))
        {
            Directory.CreateDirectory(projectPath + "/docker");
        }

        projectPath += "/docker";

        var tar = await service.ExportContainerAsTarObjectAsync(containerName, projectPath + "/export.tar");

        // salvo il file tar ricevuto nella cartella docker del progetto
        using (var fileStream = File.Create(projectPath + "/export.tar"))
        {
            tar.CopyTo(fileStream);
        }
    }
}
