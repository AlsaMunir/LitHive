using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Application.Services
{
    public class GenericService<TEntity> where TEntity : class
    {
        private readonly IRepository<TEntity> irepo;

        public GenericService(IRepository<TEntity> irepo)
        {
            this.irepo = irepo;
        }

        public async Task DeleteAsync(int id)
        {
            await irepo.Delete(id);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await irepo.GetById(id);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await irepo.Update(entity);
        }

        public async Task AddAsync(TEntity entity)
        {
            await irepo.Add(entity);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await irepo.GetAll();
        }
    }
}
