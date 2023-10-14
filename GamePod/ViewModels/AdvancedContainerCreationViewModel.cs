using CommunityToolkit.Mvvm.ComponentModel;
using GamePod.Services;

namespace GamePod.ViewModels;

public partial class AdvancedContainerCreationViewModel : ObservableRecipient
{
    public AdvancedContainerCreationViewModel()
    {
    }

    internal void CreateContainer(string command)
    {
        // create the container with the command
        ContainerService containerService = new ContainerService(command);
        containerService.RunCommand();
    }
}
