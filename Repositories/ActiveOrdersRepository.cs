using Microsoft.EntityFrameworkCore;

namespace Project.MVC.Repositories
{
    public class ActiveOrdersRepository : IActiveOrdersRepository
    {
        private readonly ApplicationDbContext _db;
        public ActiveOrdersRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Order>> ActiveOrders()
        {
            var orders = await _db.Orders
                                  .Include(x => x.OrderDetail)
                                  .ThenInclude(x => x.Product)
                                  .ThenInclude(x => x.Category)
                                  .Include(x => x.OrderStatus)
                                  .ToListAsync();
            return orders;
        }
    }
}
