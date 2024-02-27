using System.Collections.Generic;
using MySqlConnector;
using ProductShop.Models;
using ReactiveUI;

namespace ProductShop.ViewModels;

public class AddAndEditProductItemViewModel : ReactiveObject
{
    public ProductPreview Product
    {
        get => _product;
        set => _product = value;
    }
    private ProductPreview _product = new ProductPreview();

    public AddAndEditProductItemViewModel()
    {
        GetLists();
    }
    
    public void SaveProduct()
    {
        string sql = "insert into Product (Name, ManufacturerID, ProviderID, ProductCategoryID, UnitID, Description) " +
                     $"values ('{Product.ProductName}', {ItemManufacturer.Id}, {ItemProvider.Id}, {ItemProductCaegory.Id}, {ItemUnit.Id}, '{Product.ProductDescription}')";

        int Id;
        
        using (var db = new Utils.Database())
        {
            Id = db.SetData(sql);
        }
        
        using (var db = new Utils.Database())
        {
            db.SetData($"insert into Quantity values ({Id}, {Product.ProductCount})");
        }
        
        using (var db = new Utils.Database())
        {
            db.SetData($"insert into Price values ({Id}, {Product.ProductPrice})");
        }
    }

    private void GetLists()
    {
        GetManufacturers();
        GetProviders();
        GetProductCategories();
        GetUnits();
    }
    
    private void GetManufacturers()
    {
        var result = new List<Manufacturer>();

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
    
    private void GetProviders()
    {
        var result = new List<Provider>();

        string sql = "select Provider.ID as Id, Provider.Provider as Name from Provider";
        
        using (var db = new Utils.Database())
        {
            MySqlDataReader dataReader = db.GetData(sql);
            while (dataReader.Read() && dataReader.HasRows)
            {
                var currentItem = new Provider()
                {
                    Id = dataReader.GetInt32("Id"),
                    Name = dataReader.GetString("Name")
                };
                
                result.Add(currentItem);
            }
        }

        Providers = result;
    }
    
    private void GetProductCategories()
    {
        var result = new List<ProductCategory>();

        string sql = "select ProductCategory.ID as Id, ProductCategory.ProductCategory as Name from ProductCategory";
        
        using (var db = new Utils.Database())
        {
            MySqlDataReader dataReader = db.GetData(sql);
            while (dataReader.Read() && dataReader.HasRows)
            {
                var currentItem = new ProductCategory()
                {
                    Id = dataReader.GetInt32("Id"),
                    Name = dataReader.GetString("Name")
                };
                
                result.Add(currentItem);
            }
        }

        ProductCategories = result;
    }
    
    private void GetUnits()
    {
        var result= new List<Unit>();

        string sql = "select Unit.ID as Id, Unit.Unit as Name from Unit";
        
        using (var db = new Utils.Database())
        {
            MySqlDataReader dataReader = db.GetData(sql);
            while (dataReader.Read() && dataReader.HasRows)
            {
                var currentItem = new Unit()
                {
                    Id = dataReader.GetInt32("Id"),
                    Name = dataReader.GetString("Name")
                };
                
                result.Add(currentItem);
            }
        }

        Units = result;
    }
    
    private List<Manufacturer> _manufacturers = new List<Manufacturer>();
    private List<Provider> _providers = new List<Provider>();
    private List<ProductCategory> _productCategories = new List<ProductCategory>();
    private List<Unit> _units = new List<Unit>();
    public List<Manufacturer> Manufacturers
    {
        get => _manufacturers;
        set
        {
            _manufacturers = value;
            this.RaisePropertyChanged();
        }
    }
    public List<Provider> Providers
    {
        get => _providers;
        set
        {
            _providers = value;
            this.RaisePropertyChanged();
        }
    }
    public List<ProductCategory> ProductCategories
    {
        get => _productCategories;
        set
        {
            _productCategories = value;
            this.RaisePropertyChanged();
        }
    }
    public List<Unit> Units
    {
        get => _units;
        set
        {
            _units = value;
            this.RaisePropertyChanged();
        }
    }
    
    private Manufacturer _itemManufacturer { get; set; }
    private Provider _itemProvider { get; set; }
    private ProductCategory _itemProductCaegory { get; set; }
    private Unit _itemUnit { get; set; }

    public Manufacturer ItemManufacturer
    {
        get => _itemManufacturer;
        set
        {
            _itemManufacturer = value;
            this.RaisePropertyChanged();
        }
    }

    public Provider ItemProvider    
    {
        get => _itemProvider;
        set
        {
            _itemProvider = value;
            this.RaisePropertyChanged();
        }
    }
    public ProductCategory ItemProductCaegory
    {
        get => _itemProductCaegory;
        set
        {
            _itemProductCaegory = value;
            this.RaisePropertyChanged();
        }
    }
    public Unit ItemUnit
    {
        get => _itemUnit;
        set
        {
            _itemUnit = value;
            this.RaisePropertyChanged();
        }
    }
}