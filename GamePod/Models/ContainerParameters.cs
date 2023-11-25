using System.Diagnostics;
using Docker.DotNet.Models;
using GamePod.Contracts.Services;

namespace GamePod.Models;
internal class ContainerParameters
{
    // Properties
    public string ProjectName { get; private set; } = string.Empty;
    public string ProjectPath { get; private set; } = string.Empty;
    public GameEngine ProjectGameEngine
    {
        get; private set;
    }
    public bool DestroyAfterUse { get; private set; } = false;
    public string Port { get; private set; } = string.Empty;
    public long CPUCores { get; private set; } = 0;
    public long RAM { get; private set; } = 0;
    public string GPU { get; private set; } = string.Empty;
    public string OtherFolderPath { get; private set; } = string.Empty;
    public string DestinationPath { get; private set; } = string.Empty;
    public DirectoryInfo ContainerFolderPath
    {
        get; private set;
    }

    public CreateContainerParameters ContainerParam
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
            environmentOptions.Add("NVIDIA_DRIVER_CAPABILITIES=all");
            environmentOptions.Add("NVIDIA_VISIBLE_DEVICES=all");
            return environmentOptions;
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

            deviceRequestConfig.Add(new DeviceRequest { Driver = "nvidia", Count = 1, DeviceIDs = null, Capabilities = new List<IList<string>> { new List<string> { "gpu" } } });


            return deviceRequestConfig;
        }
    }

    private IDockerService service = App.GetService<IDockerService>();
    public ContainerInspectResponse InspectResponse
    {
        get; private set;
    }

    public ContainerParameters(string projectName, string projectPath, string gameEngineName, string gameEngineVersion, bool destroyAfetrUse, string ports, long cpuCores, string ram, string otherFolder, string destinationPath)
    {
        ProjectName = projectName;
        ProjectPath = projectPath;
        ProjectGameEngine = GameEngine.GetGameEngine(gameEngineName);
        ProjectGameEngine.Version = gameEngineVersion;
        DestroyAfterUse = destroyAfetrUse;
        Port = ports;
        CPUCores = cpuCores;
        RAM = ram == "" ? 0 : Convert.ToInt64(ram.Substring(0, ram.Length - 1)) * 1073741824;
        OtherFolderPath = otherFolder;
        DestinationPath = destinationPath;

        CreateContainerParametersObject();
    }

    private void CreateContainerParametersObject()
    {
        ContainerParam = new CreateContainerParameters
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
                NanoCPUs = CPUCores,
                Memory = RAM,
                DeviceRequests = DeviceRequestConfig,

            },
            Env = EnvironmentOptions,
            Cmd = new List<string> { "bash" },
            ExposedPorts = PortExposeConfig,


        };
    }
}
