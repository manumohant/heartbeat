using Microsoft.Data.SqlClient;

namespace HDPredictor.Services
{
    public class DatabaseService
    {
        string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=hdp;Integrated Security=SSPI;";
        public DatabaseService() 
        {

        }
        public async Task<bool> LoginAsync(string username, string password)
        {
            return username.Equals("Doctor") && password.Equals("Doctor");
            // enable below logic after creating db
            var found = false;
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand comm = new Microsoft.Data.SqlClient.SqlCommand($"select * from login where username=@username and password=@password", connection);
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
