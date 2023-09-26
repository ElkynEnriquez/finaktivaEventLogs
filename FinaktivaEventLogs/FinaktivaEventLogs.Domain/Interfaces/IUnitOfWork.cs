using FinaktivaEventLogs.Domain.Interfaces.Repositories;

namespace FinaktivaEventLogs.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        #region Repositories
        IEventLogRepository EventLogRepository { get; }
        #endregion
        void Commit();
        Task<int> CommitAsync();
        void BeginTransaction();
        Task BeginTransactionAsync();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
