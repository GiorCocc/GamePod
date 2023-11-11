using System.Diagnostics;
using Docker.DotNet;
using Docker.DotNet.Models;
using GamePod.Contracts.Services;
using GamePod.Models;

/*
 *  DockerService.cs
 *  @Author: Giorgio Coccapani
 *  @Date: 14/10/2023
 *  This class will interface the app with the Docker engine using the Docker API for .NET
 *  This class will be used to start, stop, delete, etc. the containers and list all the container created by the app
 *  
 */

namespace GamePod.Services;

public class DockerService : IDockerService
{

    private DockerClient? client = null;
    public string? Version { get; set; } = null;

    public DockerService()
    {
        CreateClient();

        // get the version of the Docker engine
        _ = GetVersion();
        _ = GetSystemInfo();

        // list all the containers created
        _ = ListContainers();
    }

    private void CreateClient()
    {
        try
        {
            client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
            Debug.WriteLine("Docker client created: " + client);
        }
        catch (Exception e)
        {
            Debug.WriteLine("Error creating Docker client: " + e.Message);
        }
    }

    // This method will return a list of all the containers created (like docker ps -a)
    public async Task<IList<ContainerListResponse>> ListContainers()
    {
        if (client == null)
        {
            CreateClient();
        }

        IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
                       new ContainersListParameters()
                       {
                           Limit = 10,
                           All = true
                       });

        Debug.WriteLine("Container list: " + containers);

        return containers;
    }

    // this method will return the version number of the Docker engine
    public async Task<string> GetVersion()
    {
        if (client == null)
        {
            CreateClient();
        }

        VersionResponse version = await client.System.GetVersionAsync();

        Debug.WriteLine("Docker version: " + version.Version);

        Version = version.Version;

        return Version;
    }

    // get the system status 
    public async Task<SystemInfoResponse> GetSystemInfo()
    {

        if (client == null)
        {
            CreateClient();
        }

        SystemInfoResponse systemInfo = await client.System.GetSystemInfoAsync();

        Debug.WriteLine("System info: " + systemInfo);

        return systemInfo;
    }

    // This method will start a container with the given name
    public async Task StartContainer(string containerName)
    {

        if (client == null)
        {

            CreateClient();
        }

        var containerStatus = await client.Containers.InspectContainerAsync(containerName);
        if (containerStatus.State.Running && !containerStatus.State.Paused)
        {

            Debug.WriteLine("Container " + containerName + " is already running");
            return;
        }
        else if (containerStatus.State.Paused)
        {

            Debug.WriteLine("Container " + containerName + " is paused. Try to unpause it");
            await client.Containers.UnpauseContainerAsync(containerName);
            return;
        }
        else
        {
            await client.Containers.StartContainerAsync(containerName, new ContainerStartParameters());
            Debug.WriteLine("Container " + containerName + " started");
            return;
        }




    }

    // This method will stop a container with the given name
    public async Task StopContainer(string containerName)
    {


        if (client == null)
        {


            CreateClient();
        }

        await client.Containers.StopContainerAsync(containerName, new ContainerStopParameters());

        Debug.WriteLine("Container " + containerName + " stopped");
    }

    // This method will restart a container with the given name
    public async Task RestartContainer(string containerName)
    {
        if (client == null)
        {
            CreateClient();
        }

        await client.Containers.RestartContainerAsync(containerName, new ContainerRestartParameters());
        Debug.WriteLine("Container " + containerName + " restarted");
    }

    // This method will delete a container with the given name
    public async Task DeleteContainer(string containerName)
    {

        if (client == null)
        {

            CreateClient();
        }

        await client.Containers.RemoveContainerAsync(containerName, new ContainerRemoveParameters());
        Debug.WriteLine("Container " + containerName + " deleted");
    }

    // This method will pause a container with the given name
    public async Task PauseContainer(string containerName)
    {


        if (client == null)
        {


            CreateClient();
        }

        await client.Containers.PauseContainerAsync(containerName);
        Debug.WriteLine("Container " + containerName + " paused");
    }

    // This method will create a container with the given container object 
    public async Task CreateContainer(CreateContainerParameters parameters)
    {
        if (client == null) CreateClient();

        await client.Containers.CreateContainerAsync(parameters);
        Debug.WriteLine("Container created");
    }

    public async Task<ContainerInspectResponse> GetContainerInspect(string containerName)
    {

        if (client == null) CreateClient();

        ContainerInspectResponse inspectResponse = await client.Containers.InspectContainerAsync(containerName);
        Debug.WriteLine(inspectResponse.ToString());

        ContainerInspectParameters containerInspectParameters = new ContainerInspectParameters();

        Debug.WriteLine("-Container information: " + containerInspectParameters);

        return inspectResponse;
    }

    // TODO: creare un metodo per mostrare sullo schermo i log del container
    public async Task<Stream> GetContainerLogs(string containerID)
    {
    
        if (client == null) CreateClient();

        // get the log stream from the container
        var logs = await client.Containers.GetContainerLogsAsync(containerID, new ContainerLogsParameters()
        {
        
                   ShowStderr = true,
                   ShowStdout = true,
                   Timestamps = true
               });

        Debug.WriteLine(logs.ToString());

        return logs;
    
    }


}

