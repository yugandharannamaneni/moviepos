using BoxOffice.Model;
using System;
using System.Collections.Generic;

namespace BoxOffice.DAL.Interfaces
{
    public interface IMovieTimingsRepository
    {
        IEnumerable<Movie> GetMovies(DateTime dtSelectedDate);

        IEnumerable<TicketPricesAndTaxes> GetTicketPriceAndTax(int screenId);

        IEnumerable<MovieTimings> GetScreenMovies(int screenId, int month, int year);

        List<MovieTimingForSync> GetMovieTimingsForSync();

        int InsertMovieTiming(List<MovieTimings> movietimings);

        int InsertCustomTicketPrices(Screenclasses sc);

        void InactiveMovieTiming(int movietimingId);

        IEnumerable<Movie> GetMovieDates(int movieId, DateTime dtSelectedDate);

        int InsertMovieBooking(int movieId, int totalSeats, double amount, int screenClassId, int screenId, int movieTimeId, string seats);

        void UpdateWebMovieTimingIds(List<MovieTimingForSync> movieTimings);
    }
}