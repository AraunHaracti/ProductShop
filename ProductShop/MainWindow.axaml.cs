using Avalonia.Controls;
using Avalonia.Interactivity;
using ProductShop.Models;
using ProductShop.Utils;

namespace ProductShop;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        GuestBtn.Click += OpenProductWindow;
        LoginBtn.Click += OpenProductWindowWithAccount;
    }

    private void OpenProductWindow(object? obj, RoutedEventArgs e)
    {
        var window = ProductsWindow.GetInstance();
        window.Show();
        this.Close();
    }

    private void OpenProductWindowWithAccount(object? obj, RoutedEventArgs e)
    {
        if (LoginField.Text != null || PasswordField.Text != null)
        {
            string sql = $"SELECT " +
                         $"User.ID, " +
                         $"User.RoleID, " +
                         $"User.LastName, " +
                         $"User.FirstName, " +
                         $"User.MiddleName, " +
                         $"User.Login, " +
                         $"User.Password " +
                         $"FROM User " +
                         $"WHERE User.Login = '{LoginField.Text}' " +
                         $"AND User.Password = '{PasswordField.Text}'";

            User account = null;
        
            using (var db = new Database())
            {
                var reader = db.GetData(sql);
                while (reader.Read() && reader.HasRows)
                {
                    account = new User()
                    {
                        Id = reader.GetInt32("ID"),
                        RoleId = reader.GetInt32("RoleID"),
                        LastName = reader.GetString("LastName"),
                        FirstName = reader.GetString("FirstName"),
                        MiddleName = reader.GetString("MiddleName"),
                        Login = reader.GetString("Login"),
                        Password = reader.GetString("Password")
                    };
                }
            }
            if (account != null)
            {
                var window = ProductsWindow.GetInstance(account);
                window.Show();
                this.Close();
                return;
            }
        }

        TextBlock messageField = this.FindControl<TextBlock>("ErrorMessage");

        messageField.IsVisible = true;
        messageField.Text = "Login error";





    }
}