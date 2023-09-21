﻿using GamePod.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace GamePod.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }

    private void CreateContainerButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.CreateContainer();
    }
}
