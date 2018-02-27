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
    public class SeatLayoutConfig : ISeatLayoutConfig
    {

        private IDbConnection con;
        static string connectionString = ConfigurationManager.ConnectionStrings["BOConnectionString"].ConnectionString;

        public IEnumerable<Screen> GetScreens()
        {
            try
            {
                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_GET_SCREENS";
                return con.Query<Screen>(storedprocedure, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Screen> GetScreens(DateTime dtShowdate, int movieId)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@date", dtShowdate);
                p.Add("@FK_BOMOVIES_ID", movieId);

                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_GET_SCREENS";
                return con.Query<Screen>(storedprocedure, param:p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Screenclasses> GetScreeClasses(int screenId)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SCREENID", screenId);

                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_GET_SCREEN_CLASSES";
                return con.Query<Screenclasses>(storedprocedure, commandType: CommandType.StoredProcedure, param: param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Seat> GetScreeSeats(int screenId, int screenClass, int moviTiming)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SCREENID", screenId);
                param.Add("@SCREEN_CLASS_ID", screenClass);
                param.Add("@FK_MOVIETIMINGS_ID", moviTiming); 

                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_GET_SCREENS_SEATS";
                return con.Query<Seat>(storedprocedure, commandType: CommandType.StoredProcedure, param: param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Seat> GetVendorSeats(int vendorId, int screenId, int moviTiming)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SCREENID", screenId);
                param.Add("@VENDOR_ID", vendorId);
                param.Add("@FK_MOVIETIMINGS_ID", moviTiming);

                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_GET_VENDOR_SEAT_LAYOUT";
                return con.Query<Seat>(storedprocedure, commandType: CommandType.StoredProcedure, param: param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Vendors> GetVendors()
        {
            try
            {
                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_GET_VENDORS";
                return con.Query<Vendors>(storedprocedure, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Vendors> GetScreenTemplates(int screenId)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SCREENID", screenId);

                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_GET_VENDOR_TEMPLATES";
                return con.Query<Vendors>(storedprocedure, param:param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Seat> GetShowTickets(int screenId, int movieTimingId)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SCREEN_ID", screenId);
                param.Add("@FK_MOVIETIMINGS_ID", movieTimingId);

                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_GET_SHOW_TICKETS";
                return con.Query<Seat>(storedprocedure, commandType: CommandType.StoredProcedure, param: param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int BulkInsertSeat(Screenclasses sc)
        {
            try
            {
                // con = new SqlConnection(connectionString);
                using (SqlBulkCopy sbc = new SqlBulkCopy(connectionString))
                {

                    DataTable data = new DataTable();

                    #region datatable definition
                    DataColumn column;

                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.Int32");
                    column.ColumnName = "FK_ScreenClasses_ID";
                    data.Columns.Add(column);

                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.Int16");
                    column.ColumnName = "FK_SeatStatus_ID";
                    data.Columns.Add(column);

                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.String");
                    column.ColumnName = "RowValue";
                    data.Columns.Add(column);

                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.String");
                    column.ColumnName = "RowText";
                    data.Columns.Add(column);

                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.String");
                    column.ColumnName = "ColumnValue";
                    data.Columns.Add(column);

                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.Int16");
                    column.ColumnName = "ColumnText";
                    data.Columns.Add(column);

                    column = new DataColumn();
                    column.DataType = System.Type.GetType("System.Int16");
                    column.ColumnName = "FK_Screens_ID";
                    data.Columns.Add(column);
                    #endregion

                    foreach (Seat objSeat in sc.seats)
                    {
                        DataRow newRow = data.NewRow();
                        newRow["FK_ScreenClasses_ID"] = sc.ScreenClassId;
                        newRow["FK_SeatStatus_ID"] = !string.IsNullOrEmpty(objSeat.ColumnText) && !string.IsNullOrEmpty(objSeat.RowText) ? "1" : "0";
                        newRow["RowValue"] = objSeat.RowValue;
                        newRow["RowText"] = objSeat.RowText;
                        newRow["ColumnValue"] = objSeat.ColumnValue;
                        newRow["ColumnText"] = !string.IsNullOrEmpty(objSeat.ColumnText) && !string.IsNullOrEmpty(objSeat.RowText) ? objSeat.ColumnText : "999";
                        newRow["FK_Screens_ID"] = sc.ScreenId;
                        data.Rows.Add(newRow);
                    }

                    List<SqlBulkCopyColumnMapping> mappings = new List<SqlBulkCopyColumnMapping>();
                    SqlBulkCopyColumnMapping screenClassID = new SqlBulkCopyColumnMapping("FK_ScreenClasses_ID", "FK_ScreenClasses_ID");
                    mappings.Add(screenClassID);
                    SqlBulkCopyColumnMapping RowValue = new SqlBulkCopyColumnMapping("RowValue", "RowValue");
                    mappings.Add(RowValue);
                    SqlBulkCopyColumnMapping RowText = new SqlBulkCopyColumnMapping("RowText", "RowText");
                    mappings.Add(RowText);
                    SqlBulkCopyColumnMapping ColumnValue = new SqlBulkCopyColumnMapping("ColumnValue", "ColumnValue");
                    mappings.Add(ColumnValue);
                    SqlBulkCopyColumnMapping ColumnText = new SqlBulkCopyColumnMapping("ColumnText", "ColumnText");
                    mappings.Add(ColumnText);
                    SqlBulkCopyColumnMapping SeatStatus = new SqlBulkCopyColumnMapping("FK_SeatStatus_ID", "FK_SeatStatus_ID");
                    mappings.Add(SeatStatus);
                    SqlBulkCopyColumnMapping ScreenId = new SqlBulkCopyColumnMapping("FK_Screens_ID", "FK_Screens_ID");
                    mappings.Add(ScreenId);

                    if (mappings != null)
                    {
                        foreach (SqlBulkCopyColumnMapping map in mappings)
                        {
                            sbc.ColumnMappings.Add(map);
                        }
                    }
                    sbc.DestinationTableName = "dbo.Seats";
                    sbc.WriteToServer(data);
                    return 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteLayout(int screenId, int screenClassId)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@SCREENID", screenId);
                p.Add("@SCREEN_CLASS_ID", screenClassId);
                p.Add("@OUTPUT", dbType: DbType.Int32, direction: ParameterDirection.Output);

                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_DELETE_SCREENS_SEATS";
                con.Query<int>(storedprocedure, param : p, commandType: CommandType.StoredProcedure).FirstOrDefault();

                return p.Get<int>("@OUTPUT");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<User> LoginUser(string uname, string password)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@USER_NAME", uname);
                p.Add("@USER_PASSWORD", password);

                con = new SqlConnection(connectionString);
                const string storedprocedure = "dbo.SP_GET_USER_LOGIN";
                return con.Query<User>(storedprocedure, param:p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}