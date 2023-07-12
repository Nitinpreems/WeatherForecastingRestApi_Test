using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace WeatherForecast.Infrastructure
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(bool lazyLoad = true);
        Task<T> Get(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression);
        IQueryable<T> GetQuery(Expression<Func<T, bool>> expression = null);
        DbContext GetDbContext();
    }
}
