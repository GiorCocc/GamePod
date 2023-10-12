using System.Text.RegularExpressions;
using GamePod.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace GamePod.Views;

public sealed partial class AdvancedContainerCreationPage : Page
{
    public AdvancedContainerCreationViewModel ViewModel
    {
        get;
    }

    public AdvancedContainerCreationPage()
    {
        ViewModel = App.GetService<AdvancedContainerCreationViewModel>();
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
        {
            // add the text to the editor rich text box in order to be modified
            var command = FormatCommand((string)e.Parameter);
            editor.Document.SetText(Microsoft.UI.Text.TextSetOptions.None, command);
        }

        base.OnNavigatedTo(e);
    }

    // TODO: separare ogni parametro del comando (ovvero gli elementi che iniziano con " --" o con " -") con un a capo e un segno di tabulazione
    private string FormatCommand(string parameter)
    {
        var command = parameter;

        return command;
    }
}
