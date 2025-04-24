-- Recreate foreign key constraints

-- Orders.CustomerId references Customers.Id
ALTER TABLE Orders
ADD CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(Id);

-- OrderLines.OrderId references Orders.Id
ALTER TABLE OrderLines
ADD CONSTRAINT FK_OrderLines_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id);

-- OrderLines.ProductId references Products.Id
ALTER TABLE OrderLines
ADD CONSTRAINT FK_OrderLines_Products FOREIGN KEY (ProductId) REFERENCES Products(Id);

-- Shipments.VehicleId references Vehicles.Id
ALTER TABLE Shipments
ADD CONSTRAINT FK_Shipments_Vehicles FOREIGN KEY (VehicleId) REFERENCES Vehicles(Id);

-- Shipments.DriverId references Employees.Id
ALTER TABLE Shipments
ADD CONSTRAINT FK_Shipments_Employees FOREIGN KEY (DriverId) REFERENCES Employees(Id);

-- ShipmentOrders.ShipmentId references Shipments.Id
ALTER TABLE ShipmentOrders
ADD CONSTRAINT FK_ShipmentOrders_Shipments FOREIGN KEY (ShipmentId) REFERENCES Shipments(Id);

-- ShipmentOrders.OrderId references Orders.Id
ALTER TABLE ShipmentOrders
ADD CONSTRAINT FK_ShipmentOrders_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id);

-- Inventories.ProductId references Products.Id
ALTER TABLE Inventories
ADD CONSTRAINT FK_Inventories_Products FOREIGN KEY (ProductId) REFERENCES Products(Id);

-- Inventories.WarehouseId references Warehouses.Id
ALTER TABLE Inventories
ADD CONSTRAINT FK_Inventories_Warehouses FOREIGN KEY (WarehouseId) REFERENCES Warehouses(Id);

