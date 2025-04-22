using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AzureSqlConnectionDemo.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if there is any data already in the database
            if (context.Customers.Any() || context.Products.Any() || context.Warehouses.Any() || context.Employees.Any() || context.Orders.Any())
                return; // Data already exists, no need to seed

            // Add Customers
            var customer1 = new Customer
            {
                BedrijfsNaam = "Customer A",
                ContactPersoon = "John Doe",
                Email = "johndoe@example.com",
                TelefoonNummer = "123-456-7890",
                Adres = "123 Main St, City, Country",
                IsDeleted = false
            };

            var customer2 = new Customer
            {
                BedrijfsNaam = "Customer B",
                ContactPersoon = "Jane Smith",
                Email = "janesmith@example.com",
                TelefoonNummer = "987-654-3210",
                Adres = "456 Second St, City, Country",
                IsDeleted = false
            };

            context.Customers.AddRange(customer1, customer2);

            // Add Products
            var product1 = new Product
            {
                ProductName = "Product 1",
                WeightKg = 10,
                Material = "Metal",
                BatchNumber = 1001,
                Price = 50.0,
                Category = "Electronics",
                IsDeleted = false
            };

            var product2 = new Product
            {
                ProductName = "Product 2",
                WeightKg = 20,
                Material = "Plastic",
                BatchNumber = 1002,
                Price = 30.0,
                Category = "Toys",
                IsDeleted = false
            };

            context.Products.AddRange(product1, product2);

            // Add Warehouses
            var warehouse1 = new Warehouse
            {
                Name = "Main Warehouse",
                Location = "City, Country",
                ContactPerson = "Alice",
                Phone = "123-456-7890",
                IsDeleted = false
            };

            var warehouse2 = new Warehouse
            {
                Name = "Secondary Warehouse",
                Location = "Another City, Country",
                ContactPerson = "Bob",
                Phone = "987-654-3210",
                IsDeleted = false
            };

            context.Warehouses.AddRange(warehouse1, warehouse2);

            // Add Inventory
            context.Inventories.AddRange(
                new Inventory
                {
                    ProductId = product1.Id,
                    WarehouseId = warehouse1.Id,
                    QuantityOnHand = 100,
                    LastUpdated = DateTime.Now,
                    IsDeleted = false
                },
                new Inventory
                {
                    ProductId = product2.Id,
                    WarehouseId = warehouse2.Id,
                    QuantityOnHand = 50,
                    LastUpdated = DateTime.Now,
                    IsDeleted = false
                }
            );

            // Add Employees
            var employee1 = new Employee
            {
                Name = "Employee A",
                Role = "Driver",
                Email = "employeeA@example.com",
                IsDeleted = false
            };

            var employee2 = new Employee
            {
                Name = "Employee B",
                Role = "Driver",
                Email = "employeeB@example.com",
                IsDeleted = false
            };

            context.Employees.AddRange(employee1, employee2);

            // Add Orders
            var order1 = new Order
            {
                CustomerId = customer1.Id,
                Status = OrderStatus.Pending,
                OrderDate = DateTime.Now,
                DeliveryAddress = "123 Main St, City, Country",
                ExpectedDeliveryDate = DateTime.Now.AddDays(5),
                ActualDeliveryDate = null,
                IsDeleted = false
            };

            var order2 = new Order
            {
                CustomerId = customer2.Id,
                Status = OrderStatus.Processing,
                OrderDate = DateTime.Now,
                DeliveryAddress = "456 Second St, City, Country",
                ExpectedDeliveryDate = DateTime.Now.AddDays(3),
                ActualDeliveryDate = null,
                IsDeleted = false
            };

            context.Orders.AddRange(order1, order2);

            // Add OrderLines
            context.OrderLines.AddRange(
                new OrderLine
                {
                    OrderId = order1.Id,
                    ProductId = product1.Id,
                    Quantity = 2,
                    IsDeleted = false
                },
                new OrderLine
                {
                    OrderId = order2.Id,
                    ProductId = product2.Id,
                    Quantity = 1,
                    IsDeleted = false
                }
            );

            // Add Vehicles
            var vehicle1 = new Vehicle
            {
                LicensePlate = "ABC123",
                CapacityKg = 1000,
                Type = VehicleType.Truck,
                Status = VehicleStatus.Available,
                IsDeleted = false
            };

            var vehicle2 = new Vehicle
            {
                LicensePlate = "XYZ456",
                CapacityKg = 500,
                Type = VehicleType.Van,
                Status = VehicleStatus.InUse,
                IsDeleted = false
            };

            context.Vehicles.AddRange(vehicle1, vehicle2);

            // Add Shipments
            var shipment1 = new Shipment
            {
                VehicleId = vehicle1.Id,
                DriverId = employee1.Id,
                Status = ShipmentStatus.Preparing,
                DepartureDate = DateTime.Now,
                ExpectedDeliveryDate = DateTime.Now.AddDays(2),
                ActualDeliveryDate = null,
                IsDeleted = false
            };

            var shipment2 = new Shipment
            {
                VehicleId = vehicle2.Id,
                DriverId = employee2.Id,
                Status = ShipmentStatus.OutForDelivery,
                DepartureDate = DateTime.Now,
                ExpectedDeliveryDate = DateTime.Now.AddDays(1),
                ActualDeliveryDate = null,
                IsDeleted = false
            };

            context.Shipments.AddRange(shipment1, shipment2);

            // Add ShipmentOrders (Many-to-Many relationship between Orders and Shipments)
            context.ShipmentOrders.AddRange(
                new ShipmentOrder
                {
                    ShipmentId = shipment1.Id,
                    OrderId = order1.Id
                },
                new ShipmentOrder
                {
                    ShipmentId = shipment2.Id,
                    OrderId = order2.Id
                }
            );

            // Add Inventory Transactions
            var transaction1 = new InventoryTransaction
            {
                ProductId = product1.Id,
                Quantity = 50,
                Type = InventoryTransactionType.Outbound,
                Timestamp = DateTime.Now,
                EmployeeId = employee1.Id,
                SourceOrDestination = "Customer A",
                IsDeleted = false
            };

            var transaction2 = new InventoryTransaction
            {
                ProductId = product2.Id,
                Quantity = 30,
                Type = InventoryTransactionType.Outbound,
                Timestamp = DateTime.Now,
                EmployeeId = employee2.Id,
                SourceOrDestination = "Customer B",
                IsDeleted = false
            };

            context.InventoryTransactions.AddRange(transaction1, transaction2);

            // Save changes to the database
            context.SaveChanges();
        }
    }
}
