using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "Server=tcp:lafeberdb.database.windows.net,1433;" +
                                  "Initial Catalog=LF-Database;" +
                                  "Persist Security Info=False;" +
                                  "User ID=CloudSAdeff4fed;" +
                                  "Password=Admin123!;" +
                                  "MultipleActiveResultSets=False;" +
                                  "Encrypt=True;" +
                                  "TrustServerCertificate=False;" +
                                  "Connection Timeout=30;";


    }
}


