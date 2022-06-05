using MySqlConnector;

namespace EFPerformance;

public class AdoDataSource : IDataSource
{
    public void GetCustomers()
    {
        using var connection = new MySqlConnection(Config.ConnectionString);

        connection.Open();
        
        using var command = new MySqlCommand(Program.SQL, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            // resolve data
        }

    }
}