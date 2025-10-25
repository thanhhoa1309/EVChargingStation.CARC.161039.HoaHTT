using EVChargingStation.CARC.Domain.HoaHTT.Entities;
using EVChargingStation.CARC.Domain.HoaHTT.Enums;
using System.ComponentModel.DataAnnotations;

namespace EVChargingStation.CARC.Domain.HoaHTT.DTOs.PlanDTOs
{
    public class PlanRequestDTOs : BaseEntity
    {

        public Guid HoaHTTID { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public PlanType Type { get; set; }

        [Range(0, 1000000)]
        public decimal? Price { get; set; }

        [Range(0, 10000)]
        public decimal? MaxDailyKwh { get; set; }
    }
}
