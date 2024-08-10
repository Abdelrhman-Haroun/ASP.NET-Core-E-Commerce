using MyShop.Entities.Models;

namespace MyShop.Entities.Repositories
{
    public interface IOrderDetailsRepository : IGenericRepository<OrderDetail>
    {
        void Update(OrderDetail OrderDetail);
    }
}
