using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;


public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task<List<T>> FindAsync(Expression<Func<T, bool>> expression);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    void Remove(T entity);
    void RemoveBy(Expression<Func<T, bool>> expression);
    void RemoveRange(IEnumerable<T> entities);
}
