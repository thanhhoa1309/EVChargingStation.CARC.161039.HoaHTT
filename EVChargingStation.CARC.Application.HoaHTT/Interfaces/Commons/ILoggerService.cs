﻿namespace EVChargingStation.CARC.Application.HoaHTT.Interfaces.Commons
{
    public interface ILoggerService
    {
        void Success(string msg);

        void Error(string msg);

        void Warn(string msg);

        void Info(string msg);
    }
}
