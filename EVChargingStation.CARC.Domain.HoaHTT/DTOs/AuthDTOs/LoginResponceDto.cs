using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVChargingStation.CARC.Domain.HoaHTT.DTOs.AuthDTOs
{
    public class LoginResponceDto
    {

        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

    }
}
