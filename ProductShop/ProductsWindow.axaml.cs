using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ProductShop.Models;
using ProductShop.Utils;
using ProductShop.ViewModels;

namespace ProductShop;

public partial class ProductsWindow : Window
{
    private ProductsWindow()
    {
        InitializeComponent();

        // if (account == null || account.RoleId != 1)
        // {
        //     ActionWithProductsGrid.IsVisible = false;
        // }
        // Console.WriteLine(account);
    }

    private static ProductsWindow _productsWindow = null;

    public static ProductsWindow GetInstance(User account = null)
    {
        if (_productsWindow == null)
        {
            _productsWindow = new ProductsWindow();
            _productsWindow.DataContext = new ProductsViewModel(account);;
        }
        
        return _productsWindow;
    }

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var window = new AddAndEditProductItemWindow(ActionItemEnum.Add);
        window.Show();
        this.Hide();
    }

    private void ExitBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var window = new MainWindow();
        window.Show();
        this.Hide();
    }
}