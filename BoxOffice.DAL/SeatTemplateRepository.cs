using BoxOffice.DAL.Interfaces;
using BoxOffice.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BoxOffice.DAL
{
    public class SeatTemplateRepository : ISeatTemplateRepository
    {
        private IDbConnection con;
        static string connectionString = ConfigurationManager.ConnectionStrings["BOConnectionString"].ConnectionString;

        public SeatTemplateRepository()
        {
        }

        public int insertSeatTemplate(int templateId, string seats, int screenId, int vendorId, string templateName, string unselectedSeats)
        {
            try
            {
                con = new SqlConnection(connectionString);
                var p = new DynamicParameters();
                p.Add("@TEMPLATE_ID", templateId);
                p.Add("@SCREENID", screenId);
                p.Add("@TEMPLATENAME", templateName);
                p.Add("@VENDORID", vendorId);
                p.Add("@SEATS", seats);
                p.Add("@UNSELECTED_SEATS", unselectedSeats);

                const string storedprocedure = "DBO.SP_INS_TEMPATEHELDSEATS";
                return con.Query<int>(storedprocedure, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int insertMgrHolding(List<Seat> seats, string HolderName, int MovieTimingId)
        {
            try
            {
                var finalSeats = string.Join(",", seats);
                var p = new DynamicParameters();
                p.Add("@HolderName", HolderName);
                p.Add("@MovieTimingId", MovieTimingId);
                p.Add("@Seats", finalSeats);
                p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
                const string storedprocedure = "dbo.sp_ins_MgrHeldSeats";
                return con.Query<int>(storedprocedure, p, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Theatres> GetTheatres()
        {
            try
            {
                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_GET_MOVIE_THEATRES";
                return con.Query<Theatres>(storedprocedure, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}