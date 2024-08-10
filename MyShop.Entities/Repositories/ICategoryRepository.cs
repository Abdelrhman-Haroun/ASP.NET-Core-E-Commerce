using MyShop.Entities.Models;

namespace MyShop.Entities.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        void Update(Category category);
    }
}
