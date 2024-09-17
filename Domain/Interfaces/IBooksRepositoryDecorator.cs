using Domain.Interfaces;
using Domain.Models;
namespace Infrastructure
{
    public interface IBooksRepositoryDecorator:IRepository<Books>
    {
        Task<int> Count();
        Task<List<Books>> FindByPriceRange(int minPrice, int maxPrice);
        Task<List<Books>> GetByCategory(int categoryId);
        Task<Books> GetById(int id);
        Task<List<Books>> GetRecentlyAdded(int count);
    }
}