namespace EVChargingStation.CARC.Infrastructure.HoaHTT.Interfaces
{
    public interface IClaimsService
    {
        public Guid GetCurrentUserId { get; }

        public string? IpAddress { get; }
    }
}
