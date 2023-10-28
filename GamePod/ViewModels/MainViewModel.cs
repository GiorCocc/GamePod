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

namespace GamePod.ViewModels;

public partial class MainViewModel : ObservableRecipient
{

    private IDockerService service;



    public MainViewModel()
    {
        service = App.GetService<IDockerService>();
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
}
