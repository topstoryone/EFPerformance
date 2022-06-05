using Dapper;
using MySqlConnector;

namespace EFPerformance;

public class DapperDataSource : IDataSource
{
    public void GetCustomers()
    {
        using var connection = new MySqlConnection(Config.ConnectionString);

        var customers = connection.Query<RawRecord>(Program.SQL).ToArray();

        foreach (var customer in customers)
        {
            // resolve data
        }
    }
}