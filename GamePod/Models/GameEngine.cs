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
    public static readonly GameEngine Unity        = new("Unity", "unity-hub");
    public static readonly GameEngine Godot        = new("Godot", "godot");
    public static readonly GameEngine UnrealEngine = new("Unreal Engine", "unreal-engine");

    // Properties
    public string Name
    {
        get; private set;
    }
    public string DownloadCommand
    {
        get; private set;
    }

    // Constructor
    private GameEngine(string name, string downloadCommand)
    {
        Name = name;
        DownloadCommand = downloadCommand;
    }

    // Get the GameEngine object from the name
    public static GameEngine GetGameEngine(string name)
    {
        switch (name)
        {
            case "Unity":
                return Unity;
            case "Godot":
                return Godot;
            case "Unreal Engine":
                return UnrealEngine;
            default:
                throw new ArgumentException("The game engine " + name + " is not supported");
        }
    }

    // Get the list of the names of the game engines
    public static List<string> GetGameEngineNamesList()
    {
        List<string> names = new List<string>();

        names.Add(Unity.Name);
        names.Add(Godot.Name);
        names.Add(UnrealEngine.Name);

        return names;

    }
}
