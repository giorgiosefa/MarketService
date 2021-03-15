using MarketService.Domain.Entities;
using MarketService.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketService.Repository.UnitOfWork
{
    public interface IUnitOfWorkManager
    {
        IGenericRepository<Market, int> Markets { get; }
        IGenericRepository<Company, int> Companies { get; }

        void Complete();
        void RejectChanges();
        Task CompleteAsync();
        Task RejectChangesAsync();

        Task BeginTransactionAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }

    public class UnitOfWorkManager : IUnitOfWorkManager, IDisposable
    {
        private readonly MarketDbContext context;
        private IDbContextTransaction _transaction;
        private IGenericRepository<Market, int> _markets;
        private IGenericRepository<Company, int> _companies;

        public IGenericRepository<Market, int> Markets
        {
            get
            {
                if (_markets == null)
                {
                    this._markets = new GenericRepository<Market, int>(context);
                }

                return this._markets;
            }
        }
        public IGenericRepository<Company, int> Companies
        {
            get
            {
                if (_companies == null)
                {
                    this._companies = new GenericRepository<Company, int>(context);
                }

                return this._companies;
            }
        }

        public UnitOfWorkManager(MarketDbContext context)
        {
            this.context = context;
        }

        public void BeginTransaction()
        {
            this._transaction = this.context.Database.BeginTransaction();
        }
        public void CommitTransaction()
        {
            this._transaction.Commit();
        }

        public void RollbackTransaction()
        {
            this._transaction.Rollback();
        }

        public async Task BeginTransactionAsync()
        {
            this._transaction = await this.context.Database.BeginTransactionAsync();
        }        

        public void Complete()
        {
            this.context.SaveChanges();
        }

        public async Task CompleteAsync()
        {
            await this.context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void RejectChanges()
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry in context.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }

        public async Task RejectChangesAsync()
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry in context.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        await entry.ReloadAsync();
                        break;
                }
            }
        }
    }
}
