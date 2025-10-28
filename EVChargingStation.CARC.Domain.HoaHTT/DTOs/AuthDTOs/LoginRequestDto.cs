using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVChargingStation.CARC.Domain.HoaHTT.DTOs.AuthDTOs
{
    public class LoginRequestDto
    {

        [DefaultValue("Admin@gmail.com")]
        public required string? Email { get; set; }

        [DefaultValue("Admin@123")]
        public required string? Password { get; set; }

    }
}
