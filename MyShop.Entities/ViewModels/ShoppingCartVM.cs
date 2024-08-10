using MyShop.Entities.Models;

namespace MyShop.Entities.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> CartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public decimal TotalCarts { get; set; }
    }
}
