﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using GamePod.Models;

namespace GamePod.Contracts.Services;
public interface IDockerService
{
    Task<IList<ContainerListResponse>> ListContainers();
    Task<string> GetVersion();
    Task<SystemInfoResponse> GetSystemInfo();
    Task StartContainer(string containerName);
    Task StopContainer(string containerName);
    Task RestartContainer(string containerName);
    Task DeleteContainer(string containerName);
    Task PauseContainer(string containerName);
    Task CreateContainer(CreateContainerParameters containerParameters);
    Task<ContainerInspectResponse> GetContainerInspect(string containerName);
    Task<Stream> GetContainerLogs(string containerName);
    Task PullImage(CreateContainerParameters containerParameters);
    Task<Stream> ExportContainerAsTarObjectAsync(string containerName, string destinationPath);
}
