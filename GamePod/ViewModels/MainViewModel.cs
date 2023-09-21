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
 * With this information, the program will create the Dockerfile/Podmanfile with the correct commands 
 * to install the game engine and the other tools needed to build the game and run it.
 * 
 */

using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GamePod.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    [ObservableProperty]
    private string _projectName;

    public MainViewModel()
    {
        // TODO: Add your initialization logic here

    }

    public void CreateContainer()
    {
        // print the project name in the console
        Debug.WriteLine("Project Name: " + ProjectName);
    }


}
