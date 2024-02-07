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
    public static readonly GameEngine UnityHubCLI       = new("Unity Hub (command line)", "unityci/hub:latest", "0", "unity-hub", "ms-appx:///Assets/GuidaUnityHub.md");
    public static readonly GameEngine UnityEditorCLI    = new("Unity Editor (command line)", "unityci/editor:2022.3.10f1-linux-il2cpp-2.0", "0", "unity-editor", "ms-appx:///Assets/GuidaUnityEditor.md");
    public static readonly GameEngine Unity             = new("Unity GUI (experimental)", "", "0", "UnityHub.AppImage", "ms-appx:///Assets/GuidaUnity.md");
    public static readonly GameEngine Godot             = new("Godot", "giorcocc/godot_fedora:2.0", "0", "godot", "ms-appx:///Assets/GuidaGodot.md");
    public static readonly GameEngine Pygame            = new("PyGame", "giorcocc/pygame:1.5", "0", "python", "ms-appx:///Assets/GuidaPyGame.md");
    public static readonly GameEngine Enigma            = new("Enigma", "giorcocc/enigma:1.1", "0", "enigma", "ms--appx:///Assets/GuidaEnigma.md");
    public static readonly GameEngine Blender           = new("Blender", "giorcocc/blender:1.0", "0", "blender", "ms-appx:///Assets/GuidaBlender.md");
    public static readonly GameEngine Vassal            = new("Vassal", "giorcocc/vassal:1.0", "0", "vassal", "ms-appx:///Assets/GuidaVassal.md");
    public static readonly GameEngine Defold            = new("Defold", "giorcocc/defold:1.1", "0", "defold", "ms-appx:///Assets/GuidaDefold.md");
    public static readonly GameEngine Stencyl           = new("Stencyl", "giorcocc/stencyl:1.1", "0", "stencyl", "ms-appx:///Assets/GuidaStencyl.md");

    public static readonly GameEngine Ubuntu            = new("Ubuntu (vanilla)", "ubuntu:latest", "0", "bash", "ms-appx:///Assets/GuidaBlender.md");
    public static readonly GameEngine Fedora            = new("Fedora (vanilla)", "fedora:latest", "0", "bash", "ms-appx:///Assets/GuidaBlender.md");

    // Properties
    public string Name { get; private set; }
    public string DockerImage { get; private set; }
    public string Version { get; set; }
    public string StartCommand { get; private set; }
    public string GuidePath { get; set; }
    

    // Constructor
    private GameEngine(string name, string dockerImage, string version, string startCommand, string guide)
    {
        Name = name;
        DockerImage = dockerImage;
        Version = version;
        StartCommand = startCommand;
        GuidePath = guide;
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
            "PyGame" => Pygame,
            "Enigma" => Enigma,
            "Blender" => Blender,
            "Ubuntu (to be removed)" => Ubuntu,
            "Fedora (to be removed)" => Fedora,
            "Vassal" => Vassal,
            "Defold" => Defold,
            "Stencyl" => Stencyl,
            _ => throw new ArgumentException("The game engine " + name + " is not supported"),
        };
    }

    public static string GetGuidePathFromImage(string image)
    {
        return image switch
        {
            "unityci/hub:latest" => UnityHubCLI.GuidePath,
            "unityci/editor:2022.3.10f1-linux-il2cpp-2.0" => UnityEditorCLI.GuidePath,
            "UnityHub.AppImage" => Unity.GuidePath,
            "giorcocc/godot_fedora:2.0" => Godot.GuidePath,
            "giorcocc/pygame:1.5" => Pygame.GuidePath,
            "giorcocc/enigma:1.1" => Enigma.GuidePath,
            "giorcocc/blender:1.0" => Blender.GuidePath,
            "giorcocc/vassal:1.0" => Vassal.GuidePath,
            "giorcocc/defold:1.1" => Defold.GuidePath,
            "giorcocc/stencyl:1.1" => Stencyl.GuidePath,
            "ubuntu:latest" => Ubuntu.GuidePath,
            "fedora:latest" => Fedora.GuidePath,
            _ => ""
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
        names.Add(Pygame.Name);
        names.Add(Enigma.Name);
        names.Add(Blender.Name);
        names.Add(Ubuntu.Name);
        names.Add(Fedora.Name);
        names.Add(Vassal.Name);
        names.Add(Defold.Name);
        names.Add(Stencyl.Name);

        names.Sort();

        return names;

    }
}
