using UnitOfWorkProj.Core.IRepository;

namespace UnitOfWorkProj.Core.IConfiguration
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        Task CompleteAsync();
    }
}
