using Microsoft.Data.SqlClient;

namespace HDPredictor.Services
{
    public class DatabaseService
    {
        string connectionString = "Server=tcp:heartdisease.database.windows.net,1433;Initial Catalog=heartdisease;Persist Security Info=False;User ID=mobileuser;Password=Mobile@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public DatabaseService() 
        {

        }
        public async Task<bool> LoginAsync(string username, string password)
        { 
            var found = false;
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand comm = new Microsoft.Data.SqlClient.SqlCommand($"select username from login where username=@username and password=@password", connection);
                comm.Parameters.AddWithValue("@username", username);
                comm.Parameters.AddWithValue("@password", password);
                var reader = await comm.ExecuteReaderAsync();
                while (reader.Read())
                {
                    found = true;
                }
            }
            
            return found;
        }

    }
}
