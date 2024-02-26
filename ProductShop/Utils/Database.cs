using System;
using MySqlConnector;

namespace ProductShop.Utils;

public class Database : IDisposable
{
    private MySqlConnector.MySqlConnection _connection;

    private MySqlCommand _command;
    
    // private MySqlConnector.MySqlConnectionStringBuilder _connectionString = new MySqlConnectionStringBuilder()
    // {
    //     Server = "10.10.1.24",
    //     Port = 3306,
    //     Database = "pro1_12",
    //     UserID = "user_01",
    //     Password = "user01pro"
    // };
    
    private MySqlConnector.MySqlConnectionStringBuilder _connectionString = new MySqlConnectionStringBuilder()
    {
        Server = "localhost",
        Port = 3306,
        Database = "pro1_12",
        UserID = "root",
        Password = "1234"
    };

    public Database()
    {
        _connection = new MySqlConnection(_connectionString.ConnectionString);
        Open();
    }

    public MySqlConnector.MySqlDataReader GetData(string sql)
    {
        _command = new MySqlCommand(sql, _connection);
        return _command.ExecuteReader();
    }
    
    private void Open()
    {  
       _connection.Open(); 
    }

    private void Close()
    {
        _connection.Close();
    }

    public void Dispose()
    {
        Close();
    }
}