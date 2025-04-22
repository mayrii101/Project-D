using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "Server=tcp:lafeberdb.database.windows.net,1433; " +
                                  "Initial Catalog=LF-Database; " +
                                  "Persist Security Info=False; " +
                                  "User ID=CloudSAdeff4fed; " +
                                  "Password=Admin123!; " +
                                  "MultipleActiveResultSets=False; " +
                                  "Encrypt=True; " +
                                  "TrustServerCertificate=False; " +
                                  "Connection Timeout=30;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Connection successful using SQL authentication!");

                string createTableQuery = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Orders' AND xtype='U')
                    CREATE TABLE Orders (
                        Id INT PRIMARY KEY IDENTITY(1,1)
                    );";

                using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("✅ 'Orders' table created successfully (if it didn't already exist).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}


