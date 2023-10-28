/*
 * GamePod
 * 
 * Created by Giorgio Coccapani
 * Date: 20/09/2023
 * 
 * View Model for the Home Page
 * In this page the developer can view the list of the projects created with GamePod
 * For each project the developer can start the container, stop it, delete it, etc.
 * At the top of the page there is a button to create a new project (Create Page)
 * 
 */

using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using GamePod.Contracts.Services;
using GamePod.Services;

namespace GamePod.ViewModels;

public partial class MainViewModel : ObservableRecipient
{

    private IDockerService service;

    public MainViewModel()
    {
        service = App.GetService<IDockerService>();
    }

    internal void DeleteContainer(object selectedContainer)
    {
        if (selectedContainer is string)
        {
            service.DeleteContainer(selectedContainer as string);
        }
        else
        {
            Debug.WriteLine("Selected container is not a string: " + selectedContainer);
        }
    }

    internal void PauseContainer(object selectedContainer)
    {
        if (selectedContainer is string)
        {
            service.PauseContainer(selectedContainer as string);
        }
        else
        {
            Debug.WriteLine("Selected container is not a string: " + selectedContainer);
        }
    }

    internal void RestartContainer(object selectedContainer)
    {
        if (selectedContainer is string)
        {
            service.RestartContainer(selectedContainer as string);
        }
        else
        {
            Debug.WriteLine("Selected container is not a string: " + selectedContainer);
        }
    }

    internal async void StartContainer(object selectedContainer)
    {
        if (selectedContainer is string)
        {
            await service.StartContainer(selectedContainer as string);
            
            // open a terminal to the container
            Process process = new Process();
            process.StartInfo.FileName = "wt.exe";
            process.StartInfo.Arguments = "docker exec -it " + selectedContainer + " bash";
            process.Start();
            
        }
        else
        {
            Debug.WriteLine("Selected container is not a string: " + selectedContainer);
        }
    }

    internal async void StopContainer(object selectedContainer)
    {
        if (selectedContainer is string)
        {
            await service.StopContainer(selectedContainer as string);
        } else
        {
            Debug.WriteLine("Selected container is not a string: " + selectedContainer);
        }
    }
}
