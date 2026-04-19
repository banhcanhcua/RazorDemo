using RazorDemo.Data;
using RazorDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace RazorDemo.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;
        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(int userId, int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null || product.Stock < quantity)
                throw new Exception("Sản phẩm không đủ số lượng");

            var order = new Order
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity,
                TotalPrice = product.Price * quantity,
                CreatedAt = DateTime.Now
            };
            product.Stock -= quantity;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetOrdersByUserAsync(int userId)
        {
            return await _context.Orders.Include(o => o.Product).Where(o => o.UserId == userId).ToListAsync();
        }
    }
}
