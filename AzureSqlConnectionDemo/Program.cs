using Microsoft.EntityFrameworkCore;
using AzureSqlConnectionDemo.Models;
using AzureSqlConnectionDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllers(); // Add this!
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=lafeberdb.database.windows.net,1433;" +
                         "Database=LFDatabaseAzure;" +
                         "User Id=CloudSAdeff4fed;" +
                         "Password=Admin123!;" +
                         "TrustServerCertificate=True;" +
                         "Encrypt=False;" +
                         "Connection Timeout=30;"));

// Add services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddScoped<IInventoryTransactionService, InventoryTransactionService>();

var app = builder.Build();

// Configure middleware
app.UseRouting();
app.UseAuthorization();

app.MapControllers(); // Important to expose your API endpoints!

// Optional: Test DB Connection and Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    try
    {
        context.Database.OpenConnection();
        Console.WriteLine("Connection successful!");

        //SEEDDATA uncomment to run
        //SeedData.Initialize(services, context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

app.Run();

