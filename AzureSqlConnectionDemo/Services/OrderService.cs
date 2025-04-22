using Microsoft.EntityFrameworkCore;
using AzureSqlConnectionDemo.Models;

namespace AzureSqlConnectionDemo.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(int id, Order order);
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
            return await _context.Orders.Where(o => !o.IsDeleted).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.Where(o => !o.IsDeleted).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrderAsync(int id, Order order)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.ProductLines)  //Include OrderLines
                .FirstOrDefaultAsync(o => o.Id == id);

            if (existingOrder == null || existingOrder.IsDeleted) return null;

            //order properties
            existingOrder.CustomerId = order.CustomerId;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.DeliveryAddress = order.DeliveryAddress;
            existingOrder.ExpectedDeliveryDate = order.ExpectedDeliveryDate;
            existingOrder.ActualDeliveryDate = order.ActualDeliveryDate;
            existingOrder.Status = order.Status;

            //update orderlines
            foreach (var updatedOrderLine in order.ProductLines)
            {
                var existingOrderLine = existingOrder.ProductLines
                    .FirstOrDefault(ol => ol.ProductId == updatedOrderLine.ProductId);
                if (existingOrderLine != null)
                {
                    existingOrderLine.Quantity = updatedOrderLine.Quantity;
                }
                else
                {
                    existingOrder.ProductLines.Add(updatedOrderLine);
                }
            }

            await _context.SaveChangesAsync();
            return existingOrder;
        }

        public async Task<bool> SoftDeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null || order.IsDeleted) return false;

            order.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}