using GamePod.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace GamePod.Views;

public sealed partial class HomePage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public HomePage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }

    private void CreateButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        // go to the CreatePage
        Frame.Navigate(typeof(MainPage));
    }
}
