using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet.Models;

namespace GamePod.Contracts.Services;
public interface IDockerService
{
    Task<IList<ContainerListResponse>> ListContainers();
    Task<string> GetVersion();
    Task<SystemInfoResponse> GetSystemInfo();
}
