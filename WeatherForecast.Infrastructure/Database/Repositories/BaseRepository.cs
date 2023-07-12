
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WeatherForecast.Infrastructure
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;
        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<TEntity> Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Get(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAll(bool lazyLoad)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            if (!lazyLoad)
            {
                query = GetQuery();
            }
            
            return await query.ToListAsync();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<TEntity>> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return await GetQuery(expression).ToListAsync();
        }

        public IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> expression = null)
        {
            IQueryable<TEntity> query = expression == null ? _dbContext.Set<TEntity>() : _dbContext.Set<TEntity>().Where(expression);
            IEnumerable<INavigation> navigationProperties = _dbContext.Model.FindEntityType(typeof(TEntity)).GetNavigations();

            if (navigationProperties != null)
            {
                foreach (var property in navigationProperties)
                {
                    //IEnumerable<INavigation> childNav = _dbContext.Model.FindEntityType(property.Name).GetNavigations();
                    query = query.Include(property.Name);
                }
            }
            return query;
        }
        public DbContext GetDbContext ()
        {
            return _dbContext;
        }
    }
}
