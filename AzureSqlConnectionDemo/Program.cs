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

        string[] createTableScripts =
        {
            // Customer
            @"CREATE TABLE Customer (
                Id INT PRIMARY KEY IDENTITY,
                BedrijfsNaam NVARCHAR(100) NOT NULL,
                ContactPersoon NVARCHAR(100),
                Email NVARCHAR(100),
                TelefoonNummer NVARCHAR(50),
                Adres NVARCHAR(200)
            );",

            // Product
            @"CREATE TABLE Product (
                Id INT PRIMARY KEY IDENTITY,
                ProductName NVARCHAR(100),
                WeightKg INT,
                Material NVARCHAR(100),
                BatchNumber INT,
                Price FLOAT,
                Category NVARCHAR(100)
            );",

            // Warehouse
            @"CREATE TABLE Warehouse (
                Id INT PRIMARY KEY IDENTITY,
                Name NVARCHAR(100),
                Location NVARCHAR(200),
                ContactPerson NVARCHAR(100),
                Phone NVARCHAR(50)
            );",

            // Employee
            @"CREATE TABLE Employee (
                Id INT PRIMARY KEY IDENTITY,
                Name NVARCHAR(100),
                Role NVARCHAR(50),
                Email NVARCHAR(100)
            );",

            // Inventory
            @"CREATE TABLE Inventory (
                Id INT PRIMARY KEY IDENTITY,
                ProductId INT NOT NULL,
                WarehouseId INT NOT NULL,
                QuantityOnHand INT,
                LastUpdated DATETIME,
                FOREIGN KEY (ProductId) REFERENCES Product(Id),
                FOREIGN KEY (WarehouseId) REFERENCES Warehouse(Id)
            );",

            // InventoryTransaction
            @"CREATE TABLE InventoryTransaction (
                Id INT PRIMARY KEY IDENTITY,
                ProductId INT NOT NULL,
                Quantity INT,
                Type INT,
                Timestamp DATETIME,
                EmployeeId INT NOT NULL,
                SourceOrDestination NVARCHAR(200),
                FOREIGN KEY (ProductId) REFERENCES Product(Id),
                FOREIGN KEY (EmployeeId) REFERENCES Employee(Id)
            );",

            // Order (header)
            @"CREATE TABLE [Order] (
                Id INT PRIMARY KEY IDENTITY,
                CustomerId INT NOT NULL,
                Status INT,
                OrderDate DATETIME,
                DeliveryAddress NVARCHAR(200),
                ExpectedDeliveryDate DATETIME,
                ActualDeliveryDate DATETIME NULL,
                FOREIGN KEY (CustomerId) REFERENCES Customer(Id)
            );",

            // OrderLine
            @"CREATE TABLE OrderLine (
                Id INT PRIMARY KEY IDENTITY,
                OrderId INT NOT NULL,
                ProductId INT NOT NULL,
                Quantity INT,
                LineTotal FLOAT,
                FOREIGN KEY (OrderId) REFERENCES [Order](Id),
                FOREIGN KEY (ProductId) REFERENCES Product(Id)
            );",

            // Vehicle
            @"CREATE TABLE Vehicle (
                Id INT PRIMARY KEY IDENTITY,
                LicensePlate NVARCHAR(50),
                CapacityKg INT,
                Type INT,
                Status INT
            );",

            // Shipment
            @"CREATE TABLE Shipment (
                Id INT PRIMARY KEY IDENTITY,
                VehicleId INT NOT NULL,
                DriverId INT NOT NULL,
                Status INT,
                DepartureDate DATETIME,
                ExpectedDeliveryDate DATETIME,
                ActualDeliveryDate DATETIME,
                FOREIGN KEY (VehicleId) REFERENCES Vehicle(Id),
                FOREIGN KEY (DriverId) REFERENCES Employee(Id)
            );",

            // ShipmentOrder (many-to-many relationship)
            @"CREATE TABLE ShipmentOrder (
                ShipmentId INT NOT NULL,
                OrderId INT NOT NULL,
                PRIMARY KEY (ShipmentId, OrderId),
                FOREIGN KEY (ShipmentId) REFERENCES Shipment(Id),
                FOREIGN KEY (OrderId) REFERENCES [Order](Id)
            );"
        };

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            Console.WriteLine("Connection successful!");

            foreach (string script in createTableScripts)
            {
                using SqlCommand command = new SqlCommand(script, connection);
                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Executed script:\n" + script.Split('\n')[0]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing script: {ex.Message}");
                }
            }
        }

        Console.WriteLine("Database tables created successfully.");
    }
}


