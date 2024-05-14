using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _storeDbContext;
        private Hashtable _repositories;
        public UnitOfWork(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext;
            _repositories = new Hashtable();
        }
        public Task<int> CompleteAsync()
        {
            return _storeDbContext.SaveChangesAsync();
        }

        public ValueTask DisposeAsync()
        {
            return _storeDbContext.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var repositoryType = typeof(TEntity).Name;
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IGenericRepository<TEntity>)_repositories[repositoryType]!;
            }
            
            var repository = new GenericRepository<TEntity>(_storeDbContext);
            _repositories.Add(repositoryType, repository); // add the repository to the hashtable
            return repository;
        }
    }
}
