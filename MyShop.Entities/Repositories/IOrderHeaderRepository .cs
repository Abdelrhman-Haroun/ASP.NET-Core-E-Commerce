using MyShop.Entities.Models;

namespace MyShop.Entities.Repositories
{
    public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
    {
        void Update(OrderHeader OrderHeader);
        void UpdateOredrStatus(int id, string OrderStatus, string PaymentStatus);
    }
}
