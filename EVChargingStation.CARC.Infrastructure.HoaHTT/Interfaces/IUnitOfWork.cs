using EVChargingStation.CARC.Domain.HoaHTT.Entities;

namespace EVChargingStation.CARC.Infrastructure.HoaHTT.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Plan> Plan { get; }

        IGenericRepository<User> Users { get; }

        Task<int> SaveChangesAsync();
    }
}
