using Microsoft.EntityFrameworkCore;
using AzureSqlConnectionDemo.Models;
using AzureSqlConnectionDemo.Models;

namespace AzureSqlConnectionDemo.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order?> UpdateOrderAsync(int id, Order order);
        Task<bool> SoftDeleteOrderAsync(int id);
    }

    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.ProductLines.Where(pl => !pl.IsDeleted)) // Exclude soft-deleted lines
                    .ThenInclude(pl => pl.Product)
                .Include(o => o.ShipmentOrders)
                    .ThenInclude(so => so.Shipment)
                .Where(o => !o.IsDeleted)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.ProductLines.Where(pl => !pl.IsDeleted)) // Exclude soft-deleted lines
                    .ThenInclude(pl => pl.Product)
                .Include(o => o.ShipmentOrders)
                    .ThenInclude(so => so.Shipment)
                .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            // Validate Customer exists
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == order.CustomerId);
            if (!customerExists)
                throw new Exception($"Customer with ID {order.CustomerId} not found.");

            // Validate and clean ProductLines
            foreach (var line in order.ProductLines)
            {
                var productExists = await _context.Products.AnyAsync(p => p.Id == line.ProductId);
                if (!productExists)
                    throw new Exception($"Product with ID {line.ProductId} not found.");
            }

            // Validate Shipments
            foreach (var shipmentOrder in order.ShipmentOrders)
            {
                var shipmentExists = await _context.Shipments.AnyAsync(s => s.Id == shipmentOrder.ShipmentId);
                if (!shipmentExists)
                    throw new Exception($"Shipment with ID {shipmentOrder.ShipmentId} not found.");
            }

            // Add Order — EF will link by foreign keys
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> UpdateOrderAsync(int id, Order order)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.ProductLines)
                .Include(o => o.ShipmentOrders)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (existingOrder == null) return null;

            existingOrder.CustomerId = order.CustomerId;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.DeliveryAddress = order.DeliveryAddress;
            existingOrder.ExpectedDeliveryDate = order.ExpectedDeliveryDate;
            existingOrder.ActualDeliveryDate = order.ActualDeliveryDate;
            existingOrder.Status = order.Status;

            // Update ProductLines (soft-delete strategy optional)
            existingOrder.ProductLines.Clear();
            foreach (var line in order.ProductLines)
            {
                existingOrder.ProductLines.Add(new OrderLine
                {
                    ProductId = line.ProductId,
                    Quantity = line.Quantity
                });
            }

            // Update ShipmentOrders
            existingOrder.ShipmentOrders.Clear();
            foreach (var so in order.ShipmentOrders)
            {
                existingOrder.ShipmentOrders.Add(new ShipmentOrder
                {
                    ShipmentId = so.ShipmentId,
                    OrderId = id
                });
            }

            await _context.SaveChangesAsync();
            return existingOrder;
        }

        public async Task<bool> SoftDeleteOrderAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.ProductLines)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null || order.IsDeleted) return false;

            // Use the SoftDelete method instead of setting the value directly
            order.SoftDelete();
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
