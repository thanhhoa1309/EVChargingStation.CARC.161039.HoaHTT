using EVChargingStation.CARC.Domain.HoaHTT;
using EVChargingStation.CARC.Domain.HoaHTT.Entities;
using EVChargingStation.CARC.Infrastructure.HoaHTT.Interfaces;

namespace EVChargingStation.CARC.Infrastructure.HoaHTT
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FA25_SWD392_SE161039_G6_EvChargingStation _dbContext;

        public UnitOfWork(FA25_SWD392_SE161039_G6_EvChargingStation dbContext,
            IGenericRepository<Plan> planRepository)
        {
            _dbContext = dbContext;
            Plan = planRepository;
        }
        public IGenericRepository<Plan> Plan { get; }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
