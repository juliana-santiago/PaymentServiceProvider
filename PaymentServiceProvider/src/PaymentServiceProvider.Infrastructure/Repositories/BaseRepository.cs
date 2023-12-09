using Microsoft.EntityFrameworkCore;
using PaymentServiceProvider.Infrastructure.Persistence.Context;

namespace PaymentServiceProvider.Infrastructure.Persistence.Repositories
{
    public abstract class BaseRepository<TEntity> where TEntity : class
    {
        protected DataContext _context;
        protected DbSet<TEntity> _dbSet;

        #region Constr

        protected BaseRepository(DataContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        #endregion

        #region Props
        protected IQueryable<TEntity> Table
        {
            get { return _dbSet; }
        }
        #endregion

        #region Async methods

        public virtual async Task<IEnumerable<TEntity?>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task InsertOnlyParentAsync(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Added;

            await SaveChangesAsync();
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);

            await SaveChangesAsync();
        }

        #endregion

        #region Priv methods

        protected virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
