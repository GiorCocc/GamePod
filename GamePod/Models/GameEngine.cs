using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePod.Models;

/*
 *  GameEngine.cs
 *  @Author: Giorgio Coccapani
 *  @Date: 25/09/2023
 *  
 *  This class contains all the game engines that are supported by the GamePods.
 *  For every game engine there is a name and the code of the download command
 */

internal class GameEngine
{
    // Game engines
    public static readonly GameEngine UnityHubCLI       = new("Unity Hub (command line)", "unityci/editor:2022.3.10f1-linux-il2cpp-2.0", "0", "unity-hub");
    public static readonly GameEngine UnityEditorCLI    = new("Unity Editor (command line)", "unityci/editor:2022.3.10f1-linux-il2cpp-2.0", "0", "unity-editor");
    public static readonly GameEngine Unity             = new("Unity GUI","", "0", "UnityHub.AppImage");
    public static readonly GameEngine Godot             = new("Godot", "", "0", "godot");

    // Properties
    public string Name { get; private set; }
    public string DockerImage { get; private set; }
    public string Version { get; set; }
    public string StartCommand { get; private set; }
    

    // Constructor
    private GameEngine(string name, string dockerImage, string version, string startCommand)
    {
        Name = name;
        DockerImage = dockerImage;
        Version = version;
        StartCommand = startCommand;
    }

    // Get the GameEngine object from the name
    public static GameEngine GetGameEngine(string name)
    {
        return name switch
        {
            "Unity Hub (command line)" => UnityHubCLI,
            "Unity Editor (command line)" => UnityEditorCLI,
            "Unity GUI" => Unity,
            "Godot" => Godot,
            _ => throw new ArgumentException("The game engine " + name + " is not supported"),
        };
    }

    // Get the list of the names of the game engines
    public static List<string> GetGameEngineNamesList()
    {
        List<string> names = new List<string>();

        names.Add(UnityHubCLI.Name);
        names.Add(UnityEditorCLI.Name);
        names.Add(Unity.Name);
        names.Add(Godot.Name);

        return names;

    }
}
