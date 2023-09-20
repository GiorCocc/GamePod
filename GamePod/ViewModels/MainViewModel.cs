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

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage;
using Microsoft.UI.Xaml.Controls;

namespace GamePod.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    private static readonly string[] _linuxDistributions = new string[] { "Ubuntu", "Fedora", "Arch", "Debian", "CentOS", "OpenSUSE" };
    private static readonly string[] _gameEngines = new string[] { "Unity", "Godot", "Unreal Engine", "CryEngine", "Lumberyard", "GameMaker Studio", "Construct", "RPG Maker", "Adventure Game Studio", "Ren'Py", "Twine", "RPG Toolkit", "Solarus", "GDevelop", "Stencyl", "Scratch", "GameSalad", "Clickteam Fusion", "Adventure Game Studio", "Adventure Game Studio" };

    public MainViewModel()
    {
        // TODO: Add your initialization logic here
    }

    
}
