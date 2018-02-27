using BoxOffice.Model;
using System;
using System.Collections.Generic;

namespace BoxOffice.DAL.Interfaces
{
    public interface ISeatLayoutConfig
    {
        IEnumerable<Screen> GetScreens();

        IEnumerable<Screen> GetScreens(DateTime dtShowdate, int movieId);

        IEnumerable<Screenclasses> GetScreeClasses(int screenId);

        IEnumerable<Seat> GetScreeSeats(int screenId, int screenClass, int moviTiming);

        IEnumerable<Seat> GetVendorSeats(int vendorId, int screenId, int moviTiming);

        IEnumerable<Vendors> GetVendors();

        IEnumerable<Vendors> GetScreenTemplates(int screenId);

        IEnumerable<Seat> GetShowTickets(int screenId, int movieTimingId);

        int BulkInsertSeat(Screenclasses sc);

        int DeleteLayout(int screenId, int screenClassId);
    }
}