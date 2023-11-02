using GamePod.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace GamePod.Views;

public sealed partial class ContainerDetailsPage : Page
{
    public ContainerDetailsViewModel ViewModel
    {
        get;
    }

    public ContainerDetailsPage()
    {
        ViewModel = App.GetService<ContainerDetailsViewModel>();
        InitializeComponent();
    }
}
