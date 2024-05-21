using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext) // inject the DbContext 
        {
            _dbContext = dbContext;

        }


        // without retrieving the data in nav properties
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();   
        }

        //retrieving the data in nav properties using Specification Design Pattern

        //helper method 
        private IQueryable<T> ApplySpecifications(ISpecifications<T> specifications)
        {
            return SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), specifications);
        }

        public async Task<T?> GetByIdAsyncSpec(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsyncSpec(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }

        public Task<int> GetCountWithSpecAsync(ISpecifications<T> spec)
        {
            return ApplySpecifications(spec).CountAsync();
        }

        public async Task AddAsync(T entity)
        {
            await  _dbContext.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity); 
        }
       
    }
}
