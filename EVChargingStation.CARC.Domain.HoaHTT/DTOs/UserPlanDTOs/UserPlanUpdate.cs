using EVChargingStation.CARC.Domain.HoaHTT.Enums;
using System.ComponentModel.DataAnnotations;

namespace EVChargingStation.CARC.Domain.HoaHTT.DTOs.UserPlanDTOs
{
    public class UserPlanUpdate
    {
        [Required]
        public Guid PlanId { get; set; }

        [Required]
        public PlanType Type { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
