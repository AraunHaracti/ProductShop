using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MySqlConnector;
using ProductShop.Models;
using ProductShop.Utils;
using ReactiveUI;

namespace ProductShop.ViewModels;

public class ProductsViewModel : ReactiveObject
{
    private string _searchString = String.Empty;

    public string SearchString
    {
        get => _searchString;
        set
        {
            _searchString = value;
            Actions();
            this.RaisePropertyChanged();
        }
    }

    private SortEnum _currentVector = SortEnum.Asc;
    public void SortItems(SortEnum vector)
    {
        switch (vector)
        {
            default:
            case SortEnum.Asc:
                ProductsListOnView = ProductsListOnView.OrderBy(preview => preview.ProductPrice).ToList();
                break;
            case SortEnum.Desc:
                ProductsListOnView = ProductsListOnView.OrderBy(preview => preview.ProductPrice).Reverse().ToList();
                break;
        }
        this.RaisePropertyChanged(nameof(ProductsListOnView));

    }

    private void Search()
    {
        List<ProductPreview> result = new();
        
        foreach (var item in ProductsListOnView)
        {
            if (item.ProductName.Contains(SearchString) || 
                item.ProductDescription.Contains(SearchString) ||
                item.ProductManufacturer.Contains(SearchString) ||
                item.ProductPrice.ToString().Contains(SearchString) ||
                item.ProductCount.ToString().Contains(SearchString))
                result.Add(item);
        }

        ProductsListOnView = result;
    }

    private int _selectedManufacturerIndex = 0;

    public int SelectedManufacturerIndex
    {
        get => _selectedManufacturerIndex;
        set
        {
            _selectedManufacturerIndex = value;
            Actions();
            this.RaisePropertyChanged();
        }
    }

    private List<Manufacturer> _manufacturers = new List<Manufacturer>();

    public List<Manufacturer> Manufacturers
    {
        get => _manufacturers;
        set
        {
            _manufacturers = value;
            this.RaisePropertyChanged();
        }
    }

    private void Filter()
    {
        if (SelectedManufacturerIndex > 0)
        {
            ProductsListOnView = ProductsListOnView.Where(preview =>
                preview.ProductManufacturer == Manufacturers[SelectedManufacturerIndex].Name).ToList();
        }
    }
    
    public User Account { get; set; }
    public List<ProductPreview> ProductsListOnView { get => _productsListOnView;
        set
        {
            _productsListOnView = value;
            this.RaisePropertyChanged();
        } 
    }

    private List<ProductPreview> _productsListOnView = new List<ProductPreview>();
    
    private List<ProductPreview> _productPreviewsFromDb = new List<ProductPreview>();
    private List<ProductPreviewFromDB> _productPreviewFromDb;

    public ProductsViewModel()
    {
        
    }
    public ProductsViewModel(User account = null)
    {
        GetManufacturer();
        GetProductsPreviewsFromDatabase();
        _productPreviewsFromDb = ConvertProductPreviewsFromDbToProductPreviews(_productPreviewFromDb);
        
        Actions();
        
        if (account != null)
        {
            this.Account = account;
        }
    }

    private void Actions()
    {
        ProductsListOnView = _productPreviewsFromDb;
        Search();
        Filter();
        SortItems(_currentVector);
        this.RaisePropertyChanged(nameof(ProductsListOnView));
    }

    private void GetManufacturer()
    {
        var result= new List<Manufacturer>();
        result.Add(new Manufacturer() {Id = 0, Name = "Все производители"});

        string sql = "select Manufacturer.ID as Id, Manufacturer.Manufacturer as Name from Manufacturer";
        
        using (var db = new Utils.Database())
        {
            MySqlDataReader dataReader = db.GetData(sql);
            while (dataReader.Read() && dataReader.HasRows)
            {
                var currentManufacturer = new Manufacturer()
                {
                    Id = dataReader.GetInt32("Id"),
                    Name = dataReader.GetString("Name")
                };
                
                result.Add(currentManufacturer);
            }
        }

        Manufacturers = result;
    }
    
    private void GetProductsPreviewsFromDatabase()
    {
        _productPreviewFromDb = new List<ProductPreviewFromDB>();
        
        string sql = "select " +
                     "Product.Name as ProductName, " +
                     "Product.Description as ProductDescription, " +
                     "Image.Image as ProductPhoto, " +
                     "Manufacturer.Manufacturer as ProductManufacturer, " +
                     "Price.Price as ProductPrice, " +
                     "Quantity.Quantity as ProductCount " +
                     "from Product " +
                     "left join Image on Product.ID = Image.ProductID " +
                     "join Manufacturer on Product.ManufacturerID = Manufacturer.ID " +
                     "join Price on Product.ID = Price.ProductID " +
                     "join Quantity on Product.ID = Quantity.ProductID";
        
        using (var db = new Utils.Database())
        {
            MySqlDataReader dataReader = db.GetData(sql);
            while (dataReader.Read() && dataReader.HasRows)
            {
                var currentItem = new Models.ProductPreviewFromDB()
                {
                    ProductPhoto = dataReader.IsDBNull("ProductPhoto") ? "picture.png" : dataReader.GetString("ProductPhoto"),
                    ProductName = dataReader.GetString("ProductName"),
                    ProductDescription = dataReader.GetString("ProductDescription"),
                    ProductManufacturer = dataReader.GetString("ProductManufacturer"),
                    ProductPrice = dataReader.GetInt32("ProductPrice"),
                    ProductCount = dataReader.GetInt32("ProductCount")
                };
                
                _productPreviewFromDb.Add(currentItem);
            }
        }
    }

    private List<ProductPreview> ConvertProductPreviewsFromDbToProductPreviews(List<ProductPreviewFromDB> productPreviewsFromDb)
    {
        List<ProductPreview> result = new List<ProductPreview>();
        
        foreach (var item in productPreviewsFromDb)
        {
            string beginSource = "avares://ProductShop/Assets/";
            
            result.Add(new ProductPreview()
            {
                ProductPhoto = new Bitmap(AssetLoader.Open(new Uri(beginSource + item.ProductPhoto))),
                ProductName = item.ProductName,
                ProductDescription = item.ProductDescription,
                ProductManufacturer = item.ProductManufacturer,
                ProductPrice = item.ProductPrice,
                ProductCount = item.ProductCount
            });
        }

        return result;
    }
}