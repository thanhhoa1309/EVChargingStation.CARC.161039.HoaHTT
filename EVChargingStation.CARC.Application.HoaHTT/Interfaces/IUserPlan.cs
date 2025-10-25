using EVChargingStation.CARC.Domain.HoaHTT.DTOs.PlanDTOs;
using EVChargingStation.CARC.Domain.HoaHTT.DTOs.UserPlanDTOs;
using EVChargingStation.CARC.Infrastructure.HoaHTT.Commons;

namespace EVChargingStation.CARC.Application.HoaHTT.Interfaces
{
    public interface IUserPlan
    {
        Task<Pagination<ResponceUserPlanDTOs>> GetAllUserPlanAsync(
            string? search,
            string? sortBy,
            bool isDescending,
            int page,
            int pageSize
            );
        Task<ResponceUserPlanDTOs> GetUserPlanByIdAsync(Guid HoaHTTID);
        Task<ResponceUserPlanDTOs> CreateUserPlanAsync(RequestUserPlanDTOs createUserPlanDTOs);
        Task<PlanUpdateDTOs> UpdateUserPlanAsync(Guid HoaHTTID, RequestUserPlanDTOs updateUserPlanDTOs);
        Task<bool> DeleteUserPlanAsync(Guid HoaHTTID);
    }
}
