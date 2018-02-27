using BoxOffice.Model;
using System;
using System.Collections.Generic;

namespace BoxOffice.DAL.Interfaces
{
    public interface ISeatBookingRepository
    {
        int CreateTempOrder(SeatViewModel selectSeatViewModel);

        bool CancelTempOrder(int TempOrderId);

        int CreateOrder(SeatViewModel seatViewModel);

        bool CancelOrder(int orderId);
    }
}