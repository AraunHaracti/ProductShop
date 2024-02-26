namespace ProductShop.Models;

public class User
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}