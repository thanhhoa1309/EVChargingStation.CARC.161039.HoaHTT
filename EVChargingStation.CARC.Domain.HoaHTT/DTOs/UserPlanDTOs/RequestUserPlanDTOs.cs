using EVChargingStation.CARC.Domain.HoaHTT.Enums;
using System.ComponentModel.DataAnnotations;

namespace EVChargingStation.CARC.Domain.HoaHTT.DTOs.UserPlanDTOs
{
    public class RequestUserPlanDTOs
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid PlanId { get; set; }

        [Required]
        public string PlanName { get; set; } = string.Empty;

        [Required]
        public PlanType Type { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }


    }
}
