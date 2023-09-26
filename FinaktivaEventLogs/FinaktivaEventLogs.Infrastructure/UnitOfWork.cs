using FinaktivaEventLogs.Domain.Interfaces;
using FinaktivaEventLogs.Domain.Interfaces.Repositories;
using FinaktivaEventLogs.Infrastructure.Data;
using FinaktivaEventLogs.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace FinaktivaEventLogs.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly ILogger _logger;
        private IDbContextTransaction _transaction;

        public IEventLogRepository EventLogRepository { get; }
        public UnitOfWork(ApplicationDbContext appDbContext, ILoggerFactory loggerFactory)
        {
            _appDbContext = appDbContext;
            _logger = loggerFactory.CreateLogger("logs");

            #region Repositories new
            EventLogRepository = new EventLogRepository(appDbContext);
            #endregion
        }
        public void BeginTransaction()
        {
            _transaction = _appDbContext.Database.BeginTransaction();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _appDbContext.Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            try
            {
                _appDbContext.SaveChanges();
                _transaction.Commit();
            }
            finally
            {
                _transaction.Dispose();
            }
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                var result = await _appDbContext.SaveChangesAsync();
                _transaction.Commit();
                return result;
            }
            finally
            {
                _transaction.Dispose();
            }
        }

        public void CommitTransaction()
        {
            _appDbContext.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _appDbContext.Database.RollbackTransaction();
        }
    }
}