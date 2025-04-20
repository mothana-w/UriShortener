using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UriShortener.Data.Model;

namespace UriShortener.Data.Repository;

public class BaseRepository<T>(AppDbContext _dbContext) : IBaseRepository<T> where T : class
{
  public async Task AddAsync(T TEntity)
  {
    await _dbContext.Set<T>().AddAsync(TEntity);
    await _dbContext.SaveChangesAsync();
  }

  public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    => await _dbContext.Set<T>().AnyAsync(expression);

  public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> expression)
    => await _dbContext.Set<T>().FirstOrDefaultAsync(expression);

  public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> expression)
    => await _dbContext.Set<T>().SingleOrDefaultAsync(expression);

  public async Task<T?> GetSingleAsync<TProperty>(Expression<Func<T, bool>> expression, Expression<Func<T, TProperty>> navegationProp)
    => await _dbContext.Set<T>().SingleOrDefaultAsync(expression);
  
  public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
}
