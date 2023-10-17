using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using GamePod.Contracts.Services;

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
                           Limit = 10, All = true 
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

    // This method will start a contaiiner with the given name
}

