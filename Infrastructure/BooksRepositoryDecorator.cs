using Domain.Interfaces;
using Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class BooksRepositoryDecorator : IBooksRepositoryDecorator
    {
        private readonly IRepository<Books> _brepo;

        public BooksRepositoryDecorator(IRepository<Books> booksRepository)
        {
            _brepo = booksRepository;
        }

        public async Task Add(Books entity)
        {
            await _brepo.Add(entity);
        }

        public async Task Delete(int id)
        {
            await _brepo.Delete(id);
        }

        public async Task<List<Books>> GetAll()
        {
            return await _brepo.GetAll();
        }

        public async Task<Books> GetById(int id)
        {
            return await _brepo.GetById(id);
        }

        public async Task Update(Books entity)
        {
            await _brepo.Update(entity);
        }

        public async Task<List<Books>> GetByCategory(int categoryId)
        {
            var allBooks = await _brepo.GetAll();
            return allBooks.Where(b => b.CategoryId == categoryId).ToList();
        }

        public async Task<int> Count()
        {
            var allBooks = await _brepo.GetAll();
            return allBooks.Count;
        }

        public async Task<List<Books>> GetRecentlyAdded(int count)
        {
            var allBooks = await _brepo.GetAll();
            return allBooks.OrderByDescending(b => b.Id).Take(count).ToList();
        }

        public async Task<List<Books>> FindByPriceRange(int minPrice, int maxPrice)
        {
            var allBooks = await _brepo.GetAll();
            return allBooks.Where(b => b.Price >= minPrice && b.Price <= maxPrice).ToList();
        }
    }
}
