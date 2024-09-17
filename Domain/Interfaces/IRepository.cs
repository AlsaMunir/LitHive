namespace Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {

        Task Delete(int id);
        Task<TEntity> GetById(int id);
        Task Update(TEntity entity);
        Task Add(TEntity entity);
        Task<List<TEntity>> GetAll();

    }
}
