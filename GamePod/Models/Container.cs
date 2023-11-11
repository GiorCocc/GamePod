using System.ComponentModel;
using System.Diagnostics;
using Docker.DotNet.Models;
using GamePod.Contracts.Services;

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
    public long CPUCores { get; private set; } = 0;
    public long RAM { get; private set; } = 0;
    public string GPU { get; private set; } = string.Empty;
    public string OtherFolderPath { get; private set; } = string.Empty;
    public string DestinationPath { get; private set; } = string.Empty;
    public string ContainerName { get; private set; } = string.Empty;
    public string ContainerID { get; private set; } = string.Empty;
    public string ContainerImage { get; private set; } = string.Empty;
    public string ContainerStatus { get; private set; } = string.Empty;
    public long ContainerExitCode { get; private set; } = 0;
    public string ContainerPlatform { get; private set; } = string.Empty;
    public string ContainerFinishedAt { get; private set; } = string.Empty;
    public string ContainerCommand { get; private set; } = string.Empty;


    public string RunCommand { get; private set; } = string.Empty;
    public DirectoryInfo ContainerFolderPath
    {
        get; private set;
    }

    public CreateContainerParameters ContainerParameters
    {
        get; private set;
    }

    public string Name
    {
        get; set;
    }

    // create a list of string filled with the -v options
    public List<string> VolumeOptions
    {
        get
        {
            List<string> volumeOptions = new List<string>();
            volumeOptions.Add(ProjectPath + ":/project");
            volumeOptions.Add("/run/desktop/mnt/host/wslg/.X11-unix:/tmp/.X11-unix");
            volumeOptions.Add("/run/desktop/mnt/host/wslg:/mnt/wslg");
            
            if (OtherFolderPath != "" && DestinationPath != "")
            {
                volumeOptions.Add(OtherFolderPath + ":/" + DestinationPath);
            }

            return volumeOptions;
        }
    }

    public List<string> EnvironmentOptions
    {
        get
        {
            List<string> environmentOptions = new List<string>();
            environmentOptions.Add("DISPLAY=:0");
            environmentOptions.Add("WAYLAND_DISPLAY=wayland-0");
            environmentOptions.Add("XDG_RUNTIME_DIR=/mnt/wslg/runtime-dir");
            environmentOptions.Add("PULSE_SERVER=/mnt/wslg/PulseServer");
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

    public IDictionary<string, EmptyStruct> PortExposeConfig
    {
        get
        {
            IDictionary<string, EmptyStruct> portExposeConfig = new Dictionary<string, EmptyStruct>();

            if (Port != "")
            {
                var port = Port.Split(':')[1];

                portExposeConfig.Add(port + "/tcp", new EmptyStruct());

                Debug.WriteLine("PortExposeConfig: " + portExposeConfig);
            }

            return portExposeConfig;
        }
    }

    public IDictionary<string, IList<PortBinding>> PortBindingConfig
    {
        get
        {
            IDictionary<string, IList<PortBinding>> portBindingConfig = new Dictionary<string, IList<PortBinding>>();

            if (Port != "")
            {
                var hostPort = Port.Split(':')[0];
                var containerPort = Port.Split(':')[1];

                portBindingConfig.Add(containerPort + "/tcp", new List<PortBinding> { new PortBinding { HostPort = hostPort, HostIP = "0.0.0.0" } });

            }

            return portBindingConfig;
        }
    }

    public IList<DeviceRequest> DeviceRequestConfig
    {
        get
        {
            IList<DeviceRequest> deviceRequestConfig = new List<DeviceRequest>();

            if (GPU != "")
            {
                deviceRequestConfig.Add(new DeviceRequest { Driver = "nvidia", Count = 1, DeviceIDs = null, Capabilities = new List<IList<string>> { new List<string> { "gpu" } } });
            }

            return deviceRequestConfig;
        }
    }

    private IDockerService service = App.GetService<IDockerService>();
    public ContainerInspectResponse InspectResponse { get; private set; }

    // Constructor
    public Container(string projectName)
    {
        ProjectName = projectName;
    }

    public Container(string projectName, string projectPath, string gameEngineName, string gameEngineVersion, bool destroyAfetrUse, string ports, long cpuCores, string ram, string gpu, string otherFolder, string destinationPath)
    {
        ProjectName = projectName;
        ProjectPath = projectPath;
        ProjectGameEngine = GameEngine.GetGameEngine(gameEngineName);
        ProjectGameEngine.Version = gameEngineVersion;
        DestroyAfterUse = destroyAfetrUse;
        Port = ports;
        CPUCores = cpuCores;
        RAM = ram == "" ? 0 : Convert.ToInt64(ram.Substring(0, ram.Length - 1)) * 1073741824;
        GPU = gpu;
        OtherFolderPath = otherFolder;
        DestinationPath = destinationPath;


        //RunCommand = CreateCommand();

        CreateContainerParametersObject();

        //InspectResponse = service.GetContainerInspect(ProjectName).Result;

        //// TODO: creare un metodo che prende tutte le informazioni dal container e le mette in un oggetto Container
        //CompleteInformationFromInspect(InspectResponse);


        // crea una cartella .docker nella cartella del progetto
        //ContainerFolderPath = System.IO.Directory.CreateDirectory(ProjectPath + "\\docker");
        //Debug.WriteLine("ContainerFolderPath: " + ContainerFolderPath);
    }

    public async Task GetInspectResponse(Container container)
    {

        InspectResponse = await service.GetContainerInspect(container.ProjectName);
    }

    // TODO: creare un metodo che prende tutte le informazioni dal container e le mette in un oggetto Container
    public void CompleteInformationFromInspect(ContainerInspectResponse inspectResponse)
    {
        if(inspectResponse == null) return;

        ContainerName = inspectResponse.Name;
        ContainerImage = inspectResponse.Config.Image;
        ContainerID = inspectResponse.Config.Hostname;
        ContainerStatus = inspectResponse.State.Status;
        ContainerExitCode = inspectResponse.State.ExitCode;
        ContainerPlatform = inspectResponse.Platform;
        ContainerFinishedAt = FormatDate(InspectResponse.State.FinishedAt);
        ContainerCommand = inspectResponse.Config.Cmd[0];
    }

    private string FormatDate(string date)
    {
        if (date == null) return "";

        var dateTime = DateTime.Parse(date);
        return dateTime.ToString("MM/dd/yyyy HH:mm:ss");
    }

    // TODO: creare un metodo che aggiorna stato e codice del container ad ogni modifica (es. quando viene avviato, quando viene fermato, ecc.)

    private void CreateContainerParametersObject()
    {
        ContainerParameters = new CreateContainerParameters
        {
            Name = ProjectName,
            Image = ProjectGameEngine.DockerImage,
            Tty = true,
            OpenStdin = true,
            StdinOnce = true,
            HostConfig = new HostConfig
            {
                Privileged = true,
                AutoRemove = DestroyAfterUse,
                Binds = VolumeOptions,
                PortBindings = PortBindingConfig,
                // TODO:
                // CpuQuota = CPU passata in fase di creazione
                NanoCPUs = CPUCores,
                // Memory = RAM passata in fase di creazione
                Memory = RAM,
                // Gpu = GPU passata in fase di creazione
                DeviceRequests = DeviceRequestConfig,

            },
            Env = EnvironmentOptions,
            Cmd = new List<string> { "bash" },

            // network: imposta le porte da aprire per il container (come nel comando docker run --port 8080:80)
            ExposedPorts = PortExposeConfig,


        };
    }

}
