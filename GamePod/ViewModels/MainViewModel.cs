/*
 * GamePod
 * 
 * Created by Giorgio Coccapani
 * Date: 20/09/2023
 * 
 * View Model for the Home Page
 * In this page the developer can view the list of the projects created with GamePod
 * For each project the developer can start the container, stop it, delete it, etc.
 * At the top of the page there is a button to create a new project (Create Page)
 * 
 */

using CommunityToolkit.Mvvm.ComponentModel;

namespace GamePod.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    public MainViewModel()
    {
    }
}
