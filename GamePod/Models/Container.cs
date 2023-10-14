using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Maps;

namespace GamePod.Models;

/*
 *  Container.cs
 *  @Author: Giorgio Coccapani
 *  @Date: 25/09/2023
 *  
 *  This class contains the container definition for the GamePods.
 *  The container is the virtual machine that will be used to run the game engine.
 *  This class gets, from the HomePage, this information:
 *  - The name of the project (the name of the container)
 *  - The path of the project
 *  - The name of the Linux distribution
 *  - The version of the Linux distribution
 *  - The name of the game engine
 */

internal class Container
{
    // Properties
    public string ProjectName { get; private set; } = string.Empty;
    public string ProjectPath { get; private set; } = string.Empty;
    public GameEngine ProjectGameEngine { get; private set; }
    public bool DestroyAfterUse { get; private set; } = false;
    public string Port { get; private set; } = string.Empty;
    public string CPUCores { get; private set; } = string.Empty;
    public string RAM { get; private set; } = string.Empty;
    public string GPU { get; private set; } = string.Empty;
    public string OtherFolderPath { get; private set; } = string.Empty;
    public string DestinationPath { get; private set; } = string.Empty;


    public string RunCommand { get; private set; } = string.Empty;
    public DirectoryInfo ContainerFolderPath { get; private set; }

    // create a list of string filled with the -v options
    public List<string> VolumeOptions
    {
        get
        {
            List<string> volumeOptions = new List<string>();
            volumeOptions.Add("--volume /run/desktop/mnt/host/wslg/.X11-unix:/tmp/.X11-unix ");
            volumeOptions.Add("--volume /run/desktop/mnt/host/wslg:/mnt/wslg ");
            volumeOptions.Add("--volume " + ProjectPath + ":/project ");
            if (OtherFolderPath != "" && DestinationPath != "")
            {
                volumeOptions.Add("--volume " + OtherFolderPath + ":" + DestinationPath + " ");
            }
            
            return volumeOptions;
        }
    }

    public List<string> EnvironmentOptions
    {
        get
        {
            List<string> environmentOptions = new List<string>();
            environmentOptions.Add("--environment DISPLAY=:0 ");
            environmentOptions.Add("--environment WAYLAND_DISPLAY=wayland-0 ");
            environmentOptions.Add("--environment XDG_RUNTIME_DIR=/mnt/wslg/runtime-dir ");
            environmentOptions.Add("--environment PULSE_SERVER=/mnt/wslg/PulseServer ");
            return environmentOptions;
        }
    }

    // List of all docker run parameters with theri name and the command to use them (eg. "Delete the container after use" and "--rm")
    public List<(string, string)> DockerRunParameters
    {
        get
        {
            List<(string, string)> dockerRunParameters = new List<(string, string)>();
            dockerRunParameters.Add(("Volume", "--volume"));
            dockerRunParameters.Add(("Environment", "--environment"));
            dockerRunParameters.Add(("Port", "--port"));
            dockerRunParameters.Add(("CPU Cores", "--cpus"));
            dockerRunParameters.Add(("RAM", "--memory"));
            dockerRunParameters.Add(("GPU", "--gpus"));
            dockerRunParameters.Add(("Destroy after use", "--rm"));

            return dockerRunParameters;
        }
    }

    // Constructor
    public Container(string projectName, string projectPath, string gameEngineName, string gameEngineVersion, bool destroyAfetrUse, string ports, string cpuCores, string ram, string gpu, string otherFolder, string destinationPath)
    {
        ProjectName = projectName;
        ProjectPath = projectPath;
        ProjectGameEngine = GameEngine.GetGameEngine(gameEngineName);
        ProjectGameEngine.Version = gameEngineVersion;
        DestroyAfterUse = destroyAfetrUse;
        Port = ports;
        CPUCores = cpuCores;
        RAM = ram;
        GPU = gpu;
        OtherFolderPath = otherFolder;
        DestinationPath = destinationPath;


        RunCommand = CreateCommand();

        // crea una cartella .docker nella cartella del progetto
        ContainerFolderPath = System.IO.Directory.CreateDirectory(ProjectPath + "\\docker");
        Debug.WriteLine("ContainerFolderPath: " + ContainerFolderPath);
    }

    private string CreateCommand()
    {
        // create the command to create the container
        // docker run --privileged
        // --rm -it -v /run/desktop/mnt/host/wslg/.X11-unix:/tmp/.X11-unix
        // -v /run/desktop/mnt/host/wslg:/mnt/wslg
        // -v <ProjectFolder>:/project
        // -e DISPLAY=:0 -e WAYLAND_DISPLAY=wayland-0
        // -e XDG_RUNTIME_DIR=/mnt/wslg/runtime-dir
        // -e PULSE_SERVER=/mnt/wslg/PulseServer
        // --gpus all
        // --name <ProjectName> unityci/editor:2022.3.10f1-linux-il2cpp-2.0 bash
        var runCommand = "docker run --privileged --interactive --tty ";
        if (DestroyAfterUse) { runCommand += "--rm "; }

        foreach (var volumeOption in VolumeOptions)
        {
            runCommand += volumeOption;
        }

        foreach (var environmentOption in EnvironmentOptions)
        {
            runCommand += environmentOption;
        }

        if (Port != "") { runCommand += "--ports " + Port + " "; }

        if (CPUCores != "") { runCommand += "--cpus " + CPUCores + " "; }

        if (RAM != "") { runCommand += "--memory " + RAM + " "; }

        if (GPU != "") { runCommand += "--gpus all "; } 
        
        
        runCommand += "--name " + ProjectName + " ";
        runCommand += ProjectGameEngine.DockerImage + " bash";

        return runCommand;
    }

}
