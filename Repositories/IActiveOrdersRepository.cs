namespace Project.MVC.Repositories
{
    public interface IActiveOrdersRepository
    {
        Task<IEnumerable<Order>> ActiveOrders();
    }
}