namespace ProductShop.ViewModels;

public class EditProductViewModel
{
    public string? ProductPhoto { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public string ProductManufacturer { get; set; }
    public int ProductPrice { get; set; }
    public int ProductCount { get; set; }
}