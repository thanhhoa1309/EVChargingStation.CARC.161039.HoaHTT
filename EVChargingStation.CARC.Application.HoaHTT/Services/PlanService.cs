using EVChargingStation.CARC.Application.HoaHTT.Interfaces;
using EVChargingStation.CARC.Application.HoaHTT.Interfaces.Commons;
using EVChargingStation.CARC.Domain.HoaHTT.DTOs.PlanDTOs;
using EVChargingStation.CARC.Domain.HoaHTT.Entities;
using EVChargingStation.CARC.Domain.HoaHTT.Enums;
using EVChargingStation.CARC.Infrastructure.HoaHTT.Commons;
using EVChargingStation.CARC.Infrastructure.HoaHTT.Interfaces;

namespace EVChargingStation.CARC.Application.HoaHTT.Services
{
    public class PlanService : IPlanService
    {
        private readonly ILoggerService _loggerService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimsService _claimsService;

        public PlanService(ILoggerService loggerService, IUnitOfWork unitOfWork, IClaimsService claimsService)
        {
            _loggerService = loggerService;
            _unitOfWork = unitOfWork;
            _claimsService = claimsService;
        }

        public async Task<PlanResponceDTOs> CreatePlanAsync(PlanRequestDTOs planRequestDto)
        {
            _loggerService.Info($"[CreatePlanAsync] Start creating plan {planRequestDto.Name}");
            try
            {
                // ✅ Kiểm tra dữ liệu đầu vào
                if (planRequestDto.Price.HasValue && planRequestDto.Price.Value < 0)
                {
                    _loggerService.Warn($"[CreatePlanAsync] Invalid price for plan {planRequestDto.Name}");
                    throw new InvalidOperationException("Price cannot be negative.");
                }

                if (planRequestDto.MaxDailyKwh.HasValue && planRequestDto.MaxDailyKwh.Value < 0)
                {
                    _loggerService.Warn($"[CreatePlanAsync] Invalid MaxDailyKwh for plan {planRequestDto.Name}");
                    throw new InvalidOperationException("MaxDailyKwh cannot be negative.");
                }

                // ✅ Tạo mới thực thể Plan từ DTO
                var plan = new Plan
                {
                    Name = planRequestDto.Name,
                    Description = planRequestDto.Description,
                    Type = planRequestDto.Type,
                    Price = planRequestDto.Price,
                    MaxDailyKwh = planRequestDto.MaxDailyKwh,
                };

                // ✅ Thêm plan vào DB
                await _unitOfWork.Plan.AddAsync(plan);
                await _unitOfWork.SaveChangesAsync();

                _loggerService.Success($"[CreatePlanAsync] Plan {plan.Name} created successfully.");

                // ✅ Trả về DTO phản hồi
                var responseDto = new PlanResponceDTOs
                {

                    Name = plan.Name,
                    Description = plan.Description,
                    Type = plan.Type,
                    Price = plan.Price,
                    MaxDailyKwh = plan.MaxDailyKwh
                };

                return responseDto;
            }
            catch (Exception ex)
            {
                _loggerService.Error($"[CreatePlanAsync] Error creating plan. Exception: {ex.Message}");
                throw new Exception("An error occurred while creating the plan. Please try again later.");
            }
        }

        public async Task<PlanResponceDTOs> GetPlanByIdAsync(Guid HoaHTTID)
        {
            try
            {
                _loggerService.Info($"[GetPlanByIdAsync] Fetching plan details for HoaHTTID: {HoaHTTID}");

                // Lấy Plan từ database
                var plan = await _unitOfWork.Plan.GetByIdAsync(HoaHTTID);

                if (plan == null || plan.IsDeleted)
                {
                    _loggerService.Warn($"[GetPlanByIdAsync] Plan with ID {HoaHTTID} not found or has been deleted.");
                    throw new KeyNotFoundException($"Plan with ID {HoaHTTID} not found.");
                }

                // Map dữ liệu sang DTO trả về
                var responseDto = new PlanResponceDTOs
                {

                    Name = plan.Name,
                    Description = plan.Description,
                    Type = plan.Type,
                    Price = plan.Price,
                    MaxDailyKwh = plan.MaxDailyKwh
                };

                _loggerService.Success($"[GetPlanByIdAsync] Successfully retrieved details for HoaHTTID: {HoaHTTID}");
                return responseDto;
            }
            catch (KeyNotFoundException)
            {
                throw; // để controller xử lý NotFound riêng
            }
            catch (Exception ex)
            {
                _loggerService.Error($"[GetPlanByIdAsync] Error fetching plan details for HoaHTTID: {HoaHTTID}. Exception: {ex.Message}");
                throw new Exception("An error occurred while fetching plan details. Please try again later.");
            }
        }

        public async Task<Pagination<PlanResponceDTOs>> GetAllPlansAsync(
            string? search,
            string? sortBy,
            bool isDescending,
            int page,
            int pageSize,
            PlanType? type = null)
        {
            try
            {
                // 1️⃣ Lấy tất cả dữ liệu từ DB
                var plans = await _unitOfWork.Plan.GetAllAsync();
                var query = plans.AsQueryable();

                // 2️⃣ Lọc theo từ khóa tìm kiếm
                if (!string.IsNullOrWhiteSpace(search))
                {
                    var lowerSearch = search.ToLower();
                    query = query.Where(p =>
                        (!string.IsNullOrEmpty(p.Name) && p.Name.ToLower().Contains(lowerSearch)) ||
                        (!string.IsNullOrEmpty(p.Description) && p.Description.ToLower().Contains(lowerSearch))
                    );
                }

                // 3️⃣ Lọc theo loại gói (PlanType)
                if (type.HasValue)
                {
                    query = query.Where(p => p.Type == type.Value);
                }

                // 4️⃣ Tổng số gói
                var totalPlans = query.Count();

                // 5️⃣ Sắp xếp dữ liệu
                query = sortBy?.ToLower() switch
                {
                    "name" => isDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                    "price" => isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                    "maxdailykwh" => isDescending ? query.OrderByDescending(p => p.MaxDailyKwh) : query.OrderBy(p => p.MaxDailyKwh),
                    _ => query.OrderBy(p => p.HoaHTTID)
                };

                // 6️⃣ Phân trang
                var pagedPlans = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // 7️⃣ Mapping sang DTO
                var result = pagedPlans.Select(p => new PlanResponceDTOs
                {

                    Name = p.Name,
                    Description = p.Description,
                    Type = p.Type,
                    Price = p.Price,
                    MaxDailyKwh = p.MaxDailyKwh
                }).ToList();

                // 8️⃣ Tạo đối tượng phản hồi phân trang
                var response = new Pagination<PlanResponceDTOs>(result, totalPlans, page, pageSize);

                return response;
            }
            catch (Exception ex)
            {
                _loggerService.Error($"Failed to retrieve plans. Exception: {ex.Message}");
                throw new Exception("An error occurred while retrieving plans. Please try again later.");
            }
        }

        public async Task<PlanUpdateDTOs> UpdatePlanAsync(Guid HoaHTTID, PlanUpdateDTOs planUpdateDto)
        {
            try
            {
                _loggerService.Info($"[UpdatePlanAsync] Attempting to update plan info for HoaHTTID: {HoaHTTID}");

                var plan = await _unitOfWork.Plan.GetByIdAsync(HoaHTTID);
                if (plan == null || plan.IsDeleted)
                {
                    _loggerService.Warn($"[UpdatePlanAsync] Plan with ID {HoaHTTID} not found.");
                    throw new KeyNotFoundException($"Plan with ID {HoaHTTID} not found.");
                }

                bool isUpdated = false;

                // ✅ Cập nhật Name nếu có giá trị mới
                if (!string.IsNullOrWhiteSpace(planUpdateDto.Name) && plan.Name != planUpdateDto.Name)
                {
                    plan.Name = planUpdateDto.Name;
                    isUpdated = true;
                }

                // ✅ Cập nhật Description nếu có giá trị mới
                if (!string.IsNullOrWhiteSpace(planUpdateDto.Description) && plan.Description != planUpdateDto.Description)
                {
                    plan.Description = planUpdateDto.Description;
                    isUpdated = true;
                }

                // ✅ Cập nhật Type nếu hợp lệ và khác giá trị hiện tại
                if (Enum.IsDefined(typeof(PlanType), planUpdateDto.Type) && plan.Type != planUpdateDto.Type)
                {
                    plan.Type = planUpdateDto.Type;
                    isUpdated = true;
                }

                // ✅ Cập nhật Price nếu có giá trị mới và hợp lệ
                if (planUpdateDto.Price.HasValue && plan.Price != planUpdateDto.Price.Value)
                {
                    if (planUpdateDto.Price.Value < 0)
                        throw new ArgumentException("Price cannot be negative.");
                    plan.Price = planUpdateDto.Price.Value;
                    isUpdated = true;
                }

                // ✅ Cập nhật MaxDailyKwh nếu có giá trị mới và hợp lệ
                if (planUpdateDto.MaxDailyKwh.HasValue && plan.MaxDailyKwh != planUpdateDto.MaxDailyKwh.Value)
                {
                    if (planUpdateDto.MaxDailyKwh.Value < 0)
                        throw new ArgumentException("MaxDailyKwh cannot be negative.");
                    plan.MaxDailyKwh = planUpdateDto.MaxDailyKwh.Value;
                    isUpdated = true;
                }

                // ✅ Nếu không có thay đổi nào, trả về dữ liệu hiện tại
                if (!isUpdated)
                {
                    _loggerService.Warn($"[UpdatePlanAsync] No changes detected for HoaHTTID: {HoaHTTID}");
                    return new PlanUpdateDTOs
                    {
                        Name = plan.Name,
                        Description = plan.Description,
                        Type = plan.Type,
                        Price = plan.Price,
                        MaxDailyKwh = plan.MaxDailyKwh
                    };
                }

                // ✅ Cập nhật metadata
                plan.UpdatedAt = DateTime.UtcNow;
                plan.UpdatedBy = _claimsService.GetCurrentUserId;

                // ✅ Lưu thay đổi vào database
                await _unitOfWork.Plan.Update(plan);
                await _unitOfWork.SaveChangesAsync();

                _loggerService.Success($"[UpdatePlanAsync] Plan info updated successfully for HoaHTTID: {HoaHTTID}");

                // ✅ Trả về dữ liệu đã cập nhật
                return new PlanUpdateDTOs
                {
                    Name = plan.Name,
                    Description = plan.Description,
                    Type = plan.Type,
                    Price = plan.Price,
                    MaxDailyKwh = plan.MaxDailyKwh
                };
            }
            catch (Exception ex)
            {
                _loggerService.Error($"[UpdatePlanAsync] Error updating plan info for HoaHTTID: {HoaHTTID}. Exception: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeletePlanAsync(Guid HoaHTTID)
        {
            try
            {
                // Tìm plan theo Id
                var plan = await _unitOfWork.Plan.GetByIdAsync(HoaHTTID);
                if (plan == null)
                {
                    _loggerService.Warn($"Plan with ID {HoaHTTID} not found.");
                    return false;
                }

                // Thực hiện soft delete
                await _unitOfWork.Plan.SoftRemove(plan);
                await _unitOfWork.SaveChangesAsync();

                _loggerService.Info($"Successfully deleted plan with ID: {HoaHTTID}");

                return true;
            }
            catch (Exception ex)
            {
                _loggerService.Error($"An error occurred while deleting plan: {ex.Message}");
                return false;
            }
        }
    }
}