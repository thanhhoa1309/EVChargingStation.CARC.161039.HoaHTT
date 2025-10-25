using EVChargingStation.CARC.Infrastructure.HoaHTT.Interfaces;

namespace EVChargingStation.CARC.Infrastructure.HoaHTT.Commons
{
    public class CurrentTime : ICurrentTime
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.UtcNow; // Đảm bảo trả về thời gian UTC
        }
    }
}
