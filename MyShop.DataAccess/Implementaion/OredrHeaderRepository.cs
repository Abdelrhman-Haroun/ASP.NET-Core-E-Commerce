using MyShop.DataAccess.Data;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;


namespace MyShop.DataAccess.Implementaion
{
	public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository
	{
		private readonly ApplicationDbContext _context;
		public OrderHeaderRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}
		public void Update(OrderHeader OrderHeader)
		{
			_context.OrderHeaders.Update(OrderHeader);
		}

		public void UpdateOredrStatus(int id, string OrderStatus, string PaymentStatus)
		{
			var orderfromDB = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
			if (orderfromDB != null)
			{
				orderfromDB.OrderStatus = OrderStatus;
				orderfromDB.PaymentDate = DateTime.Now;
				if (PaymentStatus != null)
				{
					orderfromDB.PaymentStatus = PaymentStatus;
				}
			}
		}
	}
}
