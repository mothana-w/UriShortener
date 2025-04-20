using System.Linq.Expressions;

namespace UriShortener.Data.Repository;

public interface IBaseRepository<T> where T : class
{
  Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
  Task AddAsync(T TEntity);
  Task<T?> GetSingleAsync(Expression<Func<T, bool>> expression);
  Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> expression);
  Task SaveChangesAsync();
}