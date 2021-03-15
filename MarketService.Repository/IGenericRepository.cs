using MarketService.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarketService.Repository
{
    public interface IGenericRepository<TEntity, TKey> : IDisposable where TEntity : class
    {
        TEntity Find(TKey id);

        Task<TEntity> FindAsync(TKey id);

        IQueryable<TEntity> GetQueryable
            (
                Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                int? skip = null,
                int? take = null,
                bool asNoTracking = false,
                params Expression<Func<TEntity, object>>[] includes
            );

        //Task<IQueryable<TEntity>> GetQueryableAsync
        //   (
        //       Expression<Func<TEntity, bool>> filter = null,
        //       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        //       int? skip = null,
        //       int? take = null,
        //       params Expression<Func<TEntity, object>>[] includes
        //   );

        IEnumerable<TEntity> GetAll
        (
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<TEntity, object>>[] includes
        );

        Task<IEnumerable<TEntity>> GetAllAsync
        (
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<TEntity, object>>[] includes
        );

        IEnumerable<TEntity> Get
        (
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<TEntity, object>>[] includes
        );

        Task<IEnumerable<TEntity>> GetAsync
        (
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<TEntity, object>>[] includes
        );

        TEntity GetSingle
        (
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes
        );

        Task<TEntity> GetSingleAsync
        (
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes
        );

        TEntity GetFirst
        (
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes
        );

        Task<TEntity> GetFirstAsync
        (
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes
        );

        bool Exists(Expression<Func<TEntity, bool>> filter);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);

        int? GetCount(Expression<Func<TEntity, bool>> filter = null);

        Task<int?> GetCountAsync(Expression<Func<TEntity, bool>> filter = null);

        decimal? GetSum(Expression<Func<TEntity, decimal?>> sumProperty, Expression<Func<TEntity, bool>> filter = null);

        Task<decimal?> GetSumAsync(Expression<Func<TEntity, decimal?>> sumProperty, Expression<Func<TEntity, bool>> filter = null);

        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        // Task<TKey> AddAsync(TEntity entity);
        // Task AddRangeAsync(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        //    Task UpdateAsync(TEntity entity);
        // void UpdateRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }

    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class // where TContext : DbContext
    {
        protected readonly MarketDbContext _context;
        protected DbSet<TEntity> dbSet;
        public GenericRepository(MarketDbContext context)
        {
            _context = context;
            dbSet = context.Set<TEntity>();
        }

        public TEntity Find(TKey id)
        {
            return dbSet.Find(id);
        }

        public async Task<TEntity> FindAsync(TKey id)
        {
            return await dbSet.FindAsync(id);
        }

        public IQueryable<TEntity> GetQueryable
            (
                Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                int? skip = null,
                int? take = null,
                bool asNoTracking = false,
                params Expression<Func<TEntity, object>>[] includes
            )
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);

            }

            if (includes != null && includes.Any())
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }



        #region Read Only Operations


        public IEnumerable<TEntity> GetAll
        (
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                int? skip = default(int?),
                int? take = default(int?),
                params Expression<Func<TEntity, object>>[] includes
        )
        {
            return GetQueryable
            (
                orderBy: orderBy,
                skip: skip,
                take: take,
                includes: includes
            ).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<TEntity, object>>[] includes)
        {
            return await GetQueryable
            (
                orderBy: orderBy,
                skip: skip,
                take: take,
                includes: includes
            ).ToListAsync();
        }

        public IEnumerable<TEntity> Get
        (
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = default(int?),
            int? take = default(int?),
            params Expression<Func<TEntity, object>>[] includes
        )
        {
            return GetQueryable
            (
                filter: filter,
                orderBy: orderBy,
                skip: skip,
                take: take,
                includes: includes
            ).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            return await GetQueryable
            (
                filter: filter,
                orderBy: orderBy,
                skip: skip,
                take: take,
                includes: includes
            ).ToListAsync();
        }


        public TEntity GetFirst
        (
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes
        )
        {
            return GetQueryable
            (
                filter: filter,
                orderBy: orderBy,
                includes: includes
            ).FirstOrDefault();
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            return await GetQueryable
            (
                filter: filter,
                orderBy: orderBy,
                includes: includes
            ).FirstOrDefaultAsync();
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            return GetQueryable
            (
                filter: filter,
                includes: includes
            ).SingleOrDefault();
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            return await GetQueryable
            (
                filter: filter,
                includes: includes
            ).SingleOrDefaultAsync();
        }

        public int? GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            return GetQueryable
            (
                filter: filter
            ).Count();
        }

        public async Task<int?> GetCountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return await GetQueryable
            (
                filter: filter
            ).CountAsync();
        }

        public decimal? GetSum(Expression<Func<TEntity, decimal?>> sumProperty, Expression<Func<TEntity, bool>> filter = null)
        {

            return GetQueryable
            (
                filter: filter
            ).Sum(sumProperty);
        }

        public async Task<decimal?> GetSumAsync(Expression<Func<TEntity, decimal?>> sumProperty, Expression<Func<TEntity, bool>> filter = null)
        {
            return await GetQueryable
            (
                filter: filter
            ).SumAsync(sumProperty);
        }

        public bool Exists(Expression<Func<TEntity, bool>> filter)
        {
            return GetQueryable(filter: filter).Any();
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await GetQueryable(filter: filter).AnyAsync();
        }

        #endregion

        #region CRUD Operations

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }



        public void AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }


        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }


        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        #endregion

        public void Dispose()
        {
            _context?.Dispose();
        }

    }

}
