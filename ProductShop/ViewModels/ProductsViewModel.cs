using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MySqlConnector;
using ProductShop.Models;

namespace ProductShop.ViewModels;

public class ProductsViewModel
{
    public User Account { get; set; }
    public List<ProductPreview> ProductsList { get => _productsList; set => _productsList = value; }

    private List<ProductPreview> _productsList = new List<ProductPreview>();
    
    public ProductsViewModel(){}
    public ProductsViewModel(User account = null)
    {
        _productsList = convertProductPreviewsFromDbToProductPreviews(GetProductsPreviewsFromDatabase());

        if (account != null)
        {
            this.Account = account;
        }
    }

    private List<ProductPreviewFromDB> GetProductsPreviewsFromDatabase()
    {
        List<ProductPreviewFromDB> productPreviewFromDb = new List<ProductPreviewFromDB>();
        
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
                    ProductPhoto = dataReader.IsDBNull("ProductPhoto") ? null : dataReader.GetString("ProductPhoto"),
                    ProductName = dataReader.GetString("ProductName"),
                    ProductDescription = dataReader.GetString("ProductDescription"),
                    ProductManufacturer = dataReader.GetString("ProductManufacturer"),
                    ProductPrice = dataReader.GetInt32("ProductPrice"),
                    ProductCount = dataReader.GetInt32("ProductCount")
                };
                
                productPreviewFromDb.Add(currentItem);
            }
        }
        
        return productPreviewFromDb;
    }

    private List<ProductPreview> convertProductPreviewsFromDbToProductPreviews(List<ProductPreviewFromDB> productPreviewsFromDb)
    {
        List<ProductPreview> result = new List<ProductPreview>();

        // string[] files = Directory.GetFiles("Assets");
        
        
        foreach (var item in productPreviewsFromDb)
        {
            string beginSource = "avares://ProductShop/Assets/";

            string picturePath = beginSource + "picture.png";
            string imagePath = beginSource + item.ProductPhoto;
            
            
            // Stream photoStream = AssetLoader.Open(new Uri(imagePath));
            // if (photoStream == null)
            // {
            //     photoStream = AssetLoader.Open(new Uri(picturePath));
            // }
            
            
            result.Add(new ProductPreview()
            {
                // ProductPhoto = new Image().Source = new Bitmap(picturePath),
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