using Microsoft.Data.SqlClient;

namespace LIBRARY_MANAGEMENT_SYSTEM.backend
{
    public class dbConnection
    {
        private readonly string connectionString =
            @"Data Source=Jaydee\SQLEXPRESS;Initial Catalog=bookhiveDB;Integrated Security=True;TrustServerCertificate=True;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}