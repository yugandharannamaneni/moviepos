using BoxOffice.Sync.DAL.Interfaces;
using BoxOffice.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace BoxOffice.Sync.DAL
{
    public class MovieTimingsSyncRepository : IMovieTimingsSyncRepository
    {
        private IDbConnection con;
        static string connectionString = ConfigurationManager.ConnectionStrings["MasterDBConnectionString"].ConnectionString;

        public int InsertMovieTiming(List<MovieTimingForSync> movietimings, int TheatreId)
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
                        p.Add("@Id", movietiming.WebMovieTimingId);
                        p.Add("@ScreenId", movietiming.WebScreenId);
                        p.Add("@ShowDateTime", movietiming.ShowDateTime);
                        p.Add("@ShowDate", movietiming.ShowDate);
                        p.Add("@WebMovieId", movietiming.WebMovieId);
                        p.Add("@InternetSales", movietiming.InternetSales);
                        p.Add("@OnlineSalesDate", movietiming.OnlineSalesDate);
                        p.Add("@TheatreId", TheatreId);
                        p.Add("@IsActive", movietiming.IsActive);
                        p.Add("@ScheduleUploaded", movietiming.ScheduleUploaded);
                        p.Add("@WebMovieTimingId", movietiming.WebMovieTimingId);
                        p.Add("@IsDirty", movietiming.IsDirty);
                        p.Add("@IsReported", movietiming.IsReported);
                        p.Add("@IsClosedOnline", movietiming.IsClosedOnline);
                        p.Add("@IsLocallyReported", movietiming.IsLocallyReported);
                        p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
                        const string storedprocedure = "dbo.sp_ins_MovieTiming";
                        con.Query<int>(storedprocedure, p, commandType: CommandType.StoredProcedure).SingleOrDefault();
                        result = p.Get<int>("@Output");
                        
                        if (result > -101)
                        {
                            if (movietiming.CustomTicketPrices != null)
                            {
                                foreach (var sc in movietiming.CustomTicketPrices)
                                {
                                    sc.FK_MovieTimings_ID = result;
                                    movietiming.WebMovieTimingId = result;
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

        public int InsertCustomTicketPrices(TicketPricesAndTaxes sc)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@MovieTimingId", sc.FK_MovieTimings_ID);
                p.Add("@ScreenClassesId", sc.FK_ScreenClasses_ID);
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
    }
}