using BoxOffice.Model;
using System;
using System.Collections.Generic;

namespace BoxOffice.Sync.DAL.Interfaces
{
    public interface IMovieTimingsSyncRepository
    {
        int InsertMovieTiming(List<MovieTimingForSync> movietimings, int TheatreId);

        int InsertCustomTicketPrices(TicketPricesAndTaxes sc);

        void InactiveMovieTiming(int movietimingId);

    }
}