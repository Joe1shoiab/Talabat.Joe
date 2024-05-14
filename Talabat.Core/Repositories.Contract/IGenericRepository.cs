using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Contract
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<int> GetCountWithSpecAsync(ISpecifications<T> spec);

        Task<T?> GetByIdAsyncSpec(ISpecifications<T> spec);
        Task<IReadOnlyList<T>> GetAllAsyncSpec(ISpecifications<T> spec);

        Task AddAsync(T entity);

    }
}
