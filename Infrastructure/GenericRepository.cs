using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Infrastructure
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly string constring;

        public GenericRepository(IConfiguration configuration)
        {
            constring = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Add(TEntity entity)
        {
            var tableName = typeof(TEntity).Name;
            var properties = typeof(TEntity).GetProperties().Where(prop => prop.Name != "Id" && prop.PropertyType != typeof(IFormFile));
            var colName = string.Join(",", properties.Select(x => x.Name));
            var paramName = string.Join(",", properties.Select(y => "@" + y.Name));
            var query = $"INSERT INTO {tableName} ({colName}) VALUES ({paramName})";

            using (var con = new SqlConnection(constring))
            {
                await con.ExecuteAsync(query, entity);
            }
        }

        public async Task Delete(int id)
        {
            var tableName = typeof(TEntity).Name;
            var query = $"DELETE FROM {tableName} WHERE Id = @Id";

            using (var con = new SqlConnection(constring))
            {
                await con.ExecuteAsync(query, new { Id = id });
            }
        }

        public async Task<List<TEntity>> GetAll()
        {
            var tableName = typeof(TEntity).Name;
            var categoryTable = "Category";
            var query = $"SELECT {tableName}.*, {categoryTable}.Name as CategoryName FROM {tableName} " +
                        $"JOIN {categoryTable} ON {tableName}.CategoryId = {categoryTable}.Id";

            using (var con = new SqlConnection(constring))
            {
                var result = await con.QueryAsync<TEntity>(query);
                return result.ToList();
            }
        }

        public async Task<TEntity> GetById(int id)
        {
            var tableName = typeof(TEntity).Name;
            var query = $"SELECT * FROM {tableName} WHERE Id = @Id";

            using (var con = new SqlConnection(constring))
            {
                return await con.QueryFirstOrDefaultAsync<TEntity>(query, new { Id = id });
            }
        }

        public async Task Update(TEntity entity)
        {
            var tableName = typeof(TEntity).Name;
            var properties = typeof(TEntity).GetProperties().Where(prop => prop.Name != "Id" && prop.PropertyType != typeof(IFormFile));
            var setClause = string.Join(",", properties.Select(x => $"{x.Name} = @{x.Name}"));
            var query = $"UPDATE {tableName} SET {setClause} WHERE Id = @Id";

            using (var con = new SqlConnection(constring))
            {
                await con.ExecuteAsync(query, entity);
            }
        }
    }
}
