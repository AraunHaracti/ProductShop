using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ProductShop.Models;
using ProductShop.Utils;
using ProductShop.ViewModels;

namespace ProductShop;

public partial class AddAndEditProductItemWindow : Window
{
    public AddAndEditProductItemWindow(ActionItemEnum itemEnum)
    {
        InitializeComponent();
    }

    private void ExitBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var window = ProductsWindow.GetInstance();
        window.Show();
        this.Close();
    }
    
    
    private void ExitSaveBtn_OnClick(object? sender, RoutedEventArgs e)
    { 
        ExitBtn_OnClick(sender, e);
    }
}