using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ProductShop.Models;
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

    public static ProductsWindow GetInstance(User account = null)
    {
        ProductsWindow view = new ProductsWindow();
        ProductsViewModel viewModel = new ProductsViewModel(account);
        view.DataContext = viewModel;
        return view;
    }

    private void EditBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var window = new EditProductWindow();
        window.Show();
    }

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var window = new AddProductWindow();
        window.Show();
    }

    private void ExitBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var window = new MainWindow();
        window.Show();
        this.Close();
    }
}