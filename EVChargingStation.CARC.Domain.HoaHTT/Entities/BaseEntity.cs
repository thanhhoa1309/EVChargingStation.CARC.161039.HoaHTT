﻿using System.ComponentModel.DataAnnotations;

namespace EVChargingStation.CARC.Domain.HoaHTT.Entities
{
    public class BaseEntity
    {
        [Key] public Guid HoaHTTID { get; set; }

        // Soft delete flag
        public bool IsDeleted { get; set; }

        // Audit fields
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }

        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
