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
    public string ContainerName { get; private set; } = string.Empty;
    public string ContainerID { get; private set; } = string.Empty;
    public string ContainerImage { get; private set; } = string.Empty;
    public string ContainerStatus { get; private set; } = string.Empty;
    public long ContainerExitCode { get; private set; } = 0;
    public string ContainerPlatform { get; private set; } = string.Empty;
    public string ContainerFinishedAt { get; private set; } = string.Empty;
    public string ContainerCommand { get; private set; } = string.Empty;
    public string ContainerGuidePath { get; private set; } = string.Empty;

    private IDockerService service = App.GetService<IDockerService>();
    public ContainerInspectResponse InspectResponse
    {
        get; private set;
    }

    // Constructor
    public Container(string projectName)
    {
        ContainerName = projectName;

    }

    public async Task GetInspectResponse(Container container)
    {

        InspectResponse = await service.GetContainerInspect(container.ContainerName);
    }

    // TODO: creare un metodo che prende tutte le informazioni dal container e le mette in un oggetto Container
    public void CompleteInformationFromInspect(ContainerInspectResponse inspectResponse)
    {
        if (inspectResponse == null) return;

        ContainerName = inspectResponse.Name;
        ContainerImage = inspectResponse.Config.Image;
        ContainerID = inspectResponse.Config.Hostname;
        ContainerStatus = inspectResponse.State.Status;
        ContainerExitCode = inspectResponse.State.ExitCode;
        ContainerPlatform = inspectResponse.Platform;
        ContainerFinishedAt = FormatDate(InspectResponse.State.FinishedAt);
        ContainerCommand = inspectResponse.Config.Cmd[0];
        ContainerGuidePath = GameEngine.GetGuidePathFromImage(ContainerImage);
    }

    private string FormatDate(string date)
    {
        if (date == null) return "";

        var dateTime = DateTime.Parse(date);
        return dateTime.ToString("MM/dd/yyyy HH:mm:ss");
    }

}
