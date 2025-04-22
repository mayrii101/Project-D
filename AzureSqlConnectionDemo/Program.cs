using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AzureSqlConnectionDemo.Models;
using AzureSqlConnectionDemo.Services;
using System;

class Program
{
    static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();

            // Test database connection
            try
            {
                context.Database.OpenConnection();
                Console.WriteLine("Connection successful!");

                //SEEDDATA
                //SeedData.Initialize(services, context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                //ApplicationDbContext connectie SQL Server
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer("Server=tcp:lafeberdb.database.windows.net,1433;" +
                                         "Initial Catalog=LF-Database;" +
                                         "Persist Security Info=False;" +
                                         "User ID=CloudSAdeff4fed;" +
                                         "Password=Admin123!;" +
                                         "MultipleActiveResultSets=False;" +
                                         "Encrypt=True;" +
                                         "TrustServerCertificate=False;" +
                                         "Connection Timeout=30;"));

                //Services
                services.AddScoped<ICustomerService, CustomerService>();
                services.AddScoped<IEmployeeService, EmployeeService>();
                services.AddScoped<IOrderService, OrderService>();
                services.AddScoped<IProductService, ProductService>();
                services.AddScoped<IWarehouseService, WarehouseService>();
                services.AddScoped<IInventoryService, InventoryService>();
                services.AddScoped<IShipmentService, ShipmentService>();
                services.AddScoped<IInventoryTransactionService, InventoryTransactionService>();
            });
}


