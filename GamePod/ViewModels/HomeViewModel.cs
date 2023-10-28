
/*
 * GamePod
 * 
 * Created by Giorgio Coccapani
 * Date: 20/09/2023
 * 
 * View Model for the Create Page
 * In this page the developer can:
 * - choose the name of the project
 * - select from the file system the folder where the project exists (in this folder the Dockerfile/Podmanfile will be created)
 * - the linux distribution to use (Ubuntu, Fedora, etc.)
 * - the game engine to use (Unity, Godot, etc.)
 * - the game engine version to use (if needed)
 * - other tools to use (if needed)
 * - other settings for the container (if needed)
 * 
 * With this information, the program will open a dialog to ask the developer if he wants to create the Dockerfile/Podmanfile and save it in the project folder.
 * It will also ask the developer if he wants to create the container and run it, displaying the command to be executed in the terminal.
 * 
 * Container options:
 * - the container will be created with the --privileged option to allow the container to use the GPU
 * - the container will be created with the --network=host option to allow the container to use the host network
 * - the container will be created with the --device=/dev/dri option to allow the container to use the GPU
 * - the container will be created with the --device=/dev/snd option to allow the container to use the sound card
 * - the container will be created with the --device=/dev/input option to allow the container to use the input devices
 * - the container will be created with the --device=/dev/video0 option to allow the container to use the webcam
 * - the container will be created with the --device=/dev/usb option to allow the container to use the USB devices
 * - the container will be created with the --device=/dev/bus/usb option to allow the container to use the USB devices
 * 
 * Container configuration file parameters (Dockerfile/Podmanfile):
 * - FROM: the linux distribution to use (Ubuntu, Fedora, etc.)
 * - RUN: the commands to install the game engine and the other tools needed to build the game and run it
 * - ENV: the environment variables needed to run the game engine and the other tools
 * - VOLUME: the folder where the project exists
 * - WORKDIR: the folder where the project exists
 * - CMD: the command to run the game engine
 * - EXPOSE: the port to use to connect to the game engine
 * - ENTRYPOINT: the command to run the game engine
 * - USER: the user to use to run the game engine
 * - LABEL: the label of the container
 * 
 */

using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Docker.DotNet.Models;
using GamePod.Contracts.Services;
using GamePod.Services;
using Microsoft.UI.Xaml.Controls;

namespace GamePod.ViewModels;

public partial class HomeViewModel : ObservableRecipient
{
    [ObservableProperty]
    private string _projectName;
    [ObservableProperty]
    private string _gameEngineVersion;
    [ObservableProperty]
    private string _Port;

    private IDockerService dockerService;

    public HomeViewModel()
    {
        // TODO: Add your initialization logic here
        dockerService = App.GetService<IDockerService>();
    }

    /*
     * This method will be called when the developer clicks on the "Create Container" button
     * It will create a dialog where the developer can see the command that will be executed in the terminal in order to create the container
     * The developer can choose to execute the command or not and, if he chooses to execute it, the container will be created
     * It can also choose (with a checkbox) to create the Dockerfile/Podmanfile and save it in the project folder
     */
    public void CreateContainer(string command)
    {
        // execute the command
        ContainerService containerService = new ContainerService(command);
        Debug.WriteLine("Command: " + command);
        containerService.RunCommand();
    }

    public async Task CreateContainer(CreateContainerParameters containerParameters)
    {
        await dockerService.CreateContainer(containerParameters);

    }


}
