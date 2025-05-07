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
            // Ensure product lines are valid and products are linked
            foreach (var line in order.ProductLines)
            {
                // Fetch the product from the database
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == line.ProductId);

                if (product == null)
                {
                    throw new Exception($"Product with ID {line.ProductId} not found.");
                }

                line.Product = product; // Assign the product to the order line

                // Now you can safely access LineTotal
                double lineTotal = line.LineTotal;

                // Use the lineTotal value as needed, e.g., for adding to the total order cost
            }

            _context.Orders.Add(order); // Add the order to the context
            await _context.SaveChangesAsync(); // Save the changes to the database
            return order; // Return the created order
        }





        public async Task<Order?> UpdateOrderAsync(int id, Order order)
        {
            var existingOrder = await GetOrderByIdAsync(id);
            if (existingOrder == null) return null;

            existingOrder.CustomerId = order.CustomerId;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.DeliveryAddress = order.DeliveryAddress;
            existingOrder.ExpectedDeliveryDate = order.ExpectedDeliveryDate;
            existingOrder.ActualDeliveryDate = order.ActualDeliveryDate;
            existingOrder.Status = order.Status;

            // Remove any soft-deleted product lines (not visible in GET requests)
            existingOrder.ProductLines = order.ProductLines
                .Where(pl => !pl.IsDeleted) // Only include active lines
                .ToList();

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
