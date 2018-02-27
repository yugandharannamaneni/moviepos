using BoxOffice.DAL.Interfaces;
using BoxOffice.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace BoxOffice.DAL
{
    public class MovieTimingsRepository : IMovieTimingsRepository
    {
        private IDbConnection con;
        static string connectionString = ConfigurationManager.ConnectionStrings["BOConnectionString"].ConnectionString;

        public IEnumerable<Movie> GetMovies()
        {
            try
            {
                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_GET_MOVIES";
                return con.Query<Movie>(storedprocedure, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Movie> GetMovies(DateTime dtSelectedDate)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@DATE", dtSelectedDate);

                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_GET_MOVIES";
                return con.Query<Movie>(storedprocedure, param: p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<TicketPricesAndTaxes> GetTicketPriceAndTax(int screenId)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@Screen_ID", screenId);

                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_GET_TICKETPRICESANDTAXES";
                return con.Query<TicketPricesAndTaxes>(storedprocedure, param: p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<MovieTimings> GetScreenMovies(int screenId, int month, int year)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@SCREEN_ID", screenId);
                p.Add("@MONTH", month);
                p.Add("@YEAR", year);

                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_GET_SCREEN_MOVIE_TIMINGS";
                return con.Query<MovieTimings>(storedprocedure, param: p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertMovieTiming(List<MovieTimings> movietimings)
        {
            try
            {
                con = new SqlConnection(connectionString);
                var result = 0;

                foreach (var movietiming in movietimings)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        var p = new DynamicParameters();
                        p.Add("@MovieId", movietiming.MovieId);
                        p.Add("@ScreenId", movietiming.ScreenId);
                        p.Add("@Datetime", movietiming.ShowDateTime);
                        p.Add("@Date", movietiming.Date);
                        p.Add("@interNetSales", movietiming.IsOnline);
                        p.Add("@TemplateIds", movietiming.TemplateIds);
                        p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
                        const string storedprocedure = "dbo.sp_ins_MovieTiming";
                        con.Query<int>(storedprocedure, p, commandType: CommandType.StoredProcedure).SingleOrDefault();
                        result = p.Get<int>("@Output");
                        // return result;
                        if (result > -101)
                        {
                            if (movietiming.scs != null)
                            {
                                foreach (var sc in movietiming.scs)
                                {
                                    sc.MovieTimingId = result;
                                    var output = InsertCustomTicketPrices(sc);
                                    if (output != 100)
                                    {
                                        scope.Dispose();
                                        return output;
                                    }
                                }
                            }
                            scope.Complete();
                        }
                        else
                        {
                            scope.Dispose();
                            return result;
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertCustomTicketPrices(Screenclasses sc)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@MovieTimingId", sc.MovieTimingId);
                p.Add("@ScreenClassesId", sc.ScreenClassId);
                p.Add("@Price", sc.TicketPrice);
                p.Add("@CGST", sc.CGST);
                p.Add("@SGST", sc.SGST);
                p.Add("@MC", sc.MC);
                p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
                const string storedprocedure = "dbo.sp_ins_CustomTicketPrices";
                con.Query<int>(storedprocedure, p, commandType: CommandType.StoredProcedure).FirstOrDefault();

                return p.Get<int>("@Output");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InactiveMovieTiming(int movietimingId)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@MovieTimingId", movietimingId);

                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.sp_Inactive_Movietiming";
                con.Query(storedprocedure, param: p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Movie> GetMovieDates(int movieId, DateTime dtSelectedDate)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@FK_BOMOVIES_ID", movieId);
                p.Add("@DATE", dtSelectedDate);

                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_GET_MOVIE_DATE_AND_TIME";
                return con.Query<Movie>(storedprocedure, param: p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertMovieBooking(int movieId, int totalSeats, double amount, int screenClassId, int screenId, int movieTimeId, string seats)
        {
            try
            {
                var result = 0;
                var p = new DynamicParameters();
                p.Add("@TOTALSEATS", totalSeats);
                p.Add("@TOTALAMOUNT", amount);
                p.Add("@SCREEN_CLASS_ID", screenClassId);
                p.Add("@SCREEN_ID", screenId);
                p.Add("@MOVIE_TIMING_ID", movieTimeId);
                p.Add("@SEATS", seats);
                p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);

                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_INS_MOVIE_BOOKING";
                con.Query<int>(storedprocedure, p, commandType: CommandType.StoredProcedure).SingleOrDefault();
                result = p.Get<int>("@Output");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<MovieTimingForSync> GetMovieTimingsForSync()
        {
            try
            {
                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_GET_MOVIE_TIMINGS_AND_CUSTOMTICKETPRICES";

                var results = con.QueryMultiple(storedprocedure, commandType: CommandType.StoredProcedure);
                var movieTimings = results.Read<MovieTimingForSync>();
                var customTicketPrices = results.Read<TicketPricesAndTaxes>();

                foreach (var movieTiming in movieTimings)
                {
                    var timing = movieTiming;
                    movieTiming.CustomTicketPrices = customTicketPrices.Where(sc => sc.FK_MovieTimings_ID == timing.Id).ToList<TicketPricesAndTaxes>();
                }

                return movieTimings.ToList<MovieTimingForSync>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateWebMovieTimingIds(List<MovieTimingForSync> movieTimings)
        {
            try
            {
                foreach (var movietiming in movieTimings)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        var p = new DynamicParameters();
                        p.Add("@Id", movietiming.Id);
                        p.Add("@WebMovieTimingId", movietiming.WebMovieTimingId);
                        p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
                        const string storedprocedure = "dbo.sp_upd_MovieTiming_WebMovieTiming";
                        var result = con.Query<int>(storedprocedure, p, commandType: CommandType.StoredProcedure).SingleOrDefault();

                        if (result > -101)
                        {
                            scope.Complete();
                        }
                        else
                        {
                            scope.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}