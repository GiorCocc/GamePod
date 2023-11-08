using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using GamePod.Contracts.Services;
using GamePod.Models;

namespace GamePod.ViewModels;

public partial class ContainerDetailsViewModel : ObservableRecipient
{
    private IDockerService service;

    public ContainerDetailsViewModel()
    {
        service = App.GetService<IDockerService>();
    }

    internal async Task<Container> GetContainer(string? containerName)
    {
        Container container = new Container(containerName);
        await container.GetInspectResponse(container);

        return container;
    }
}
