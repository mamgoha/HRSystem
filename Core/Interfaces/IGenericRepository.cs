using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> : IDisposable where T: BaseEntity
    {
        Task Add(T entity);
        Task<IReadOnlyList<T>> GetAll();
        Task<T> GetById(int id);
        Task Update(T entity);
        Task Remove(T entity);
        Task<IReadOnlyList<T>> IsExists(Expression<Func<T, bool>> predicate);
        Task<int> SaveChanges();
    }
}