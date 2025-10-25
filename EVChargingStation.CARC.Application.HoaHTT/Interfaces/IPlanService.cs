using EVChargingStation.CARC.Domain.HoaHTT.DTOs.PlanDTOs;
using EVChargingStation.CARC.Domain.HoaHTT.Enums;
using EVChargingStation.CARC.Infrastructure.HoaHTT.Commons;

namespace EVChargingStation.CARC.Application.HoaHTT.Interfaces
{
    public interface IPlanService
    {
        Task<Pagination<PlanResponceDTOs>> GetAllPlansAsync(
            string? search,
            string? sortBy,
            bool isDescending,
            int page,
            int pageSize,
            PlanType? type = null);
        Task<PlanResponceDTOs> GetPlanByIdAsync(Guid HoaHTTID);
        Task<PlanResponceDTOs> CreatePlanAsync(PlanRequestDTOs planRequestDto);
        Task<PlanUpdateDTOs> UpdatePlanAsync(Guid HoaHTTID, PlanUpdateDTOs planUpdateDto);
        Task<bool> DeletePlanAsync(Guid HoaHTTID);
    }
}
