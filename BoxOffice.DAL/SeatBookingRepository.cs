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
    public class SeatBookingRepository : ISeatBookingRepository
    {
        private IDbConnection con;
        static string connectionString = ConfigurationManager.ConnectionStrings["BOConnectionString"].ConnectionString;

        public int CreateTempOrder(SeatViewModel seatViewModel)
        {
            try
            {
                con = new SqlConnection(connectionString);

                var p = new DynamicParameters();
                p.Add("@TicketCount", seatViewModel.TicketCount);
                p.Add("@ScreenId", seatViewModel.ScreenId);
                p.Add("@ScreenClassId", seatViewModel.ScreenClassId);
                p.Add("@SeatIds", seatViewModel.SeatIds);
                p.Add("@movieTimingId", seatViewModel.MovieTimingId);
                p.Add("@IPAddress", seatViewModel.IPAddress);
                p.Add("@TotalAmount", seatViewModel.TotalAmount);
                p.Add("@TicketPrice", seatViewModel.TicketPrice);
                p.Add("@WebOrderId", seatViewModel.WebOrderId);
                p.Add("@VendorId", seatViewModel.VendorId);
                p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@TempOrderId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                const string storedprocedure = "dbo.mt_ins_TempOrders";
                con.Query<int>(storedprocedure, p, commandType: CommandType.StoredProcedure).SingleOrDefault();
                var result = p.Get<int>("@Output");
                var tempOrderId = p.Get<int>("@TempOrderId");

                //Error occured or validation when result value is negative.
                if (result < 0)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    return Convert.ToInt32(tempOrderId);
                }
            }
            catch (Exception ex)
            {
                return -101;
                throw ex;
            }
        }

        public bool CancelTempOrder(int TempOrderId)
        {
            con = new SqlConnection(connectionString);

            var p = new DynamicParameters();
            p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@TempOrderId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            const string storedprocedure = "[dbo].[mt_CancelTempOrder]";
            con.Query<int>(storedprocedure, p, commandType: CommandType.StoredProcedure).SingleOrDefault();
            var result = p.Get<int>("@Output");

            return result > 0;
        }

        public int CreateOrder(SeatViewModel seatViewModel)
        {
            try
            {
                con = new SqlConnection(connectionString);

                var p = new DynamicParameters();
                p.Add("@VendorId", seatViewModel.VendorId);
                p.Add("@TempOrderId", seatViewModel.TempOrderId);
                p.Add("@WebOrderId", seatViewModel.WebOrderId);
                p.Add("@MobileNumber", seatViewModel.MobileNumber);
                p.Add("@ScreenId", seatViewModel.ScreenId);
                p.Add("@ShowDate", seatViewModel.ShowDate);
                p.Add("@ShowTime", seatViewModel.ShowTime);
                p.Add("@ScreenClassId", seatViewModel.ScreenClassId);
                p.Add("@SeatIds", seatViewModel.SeatIds);
                p.Add("@TotalAmount", seatViewModel.TotalAmount);
                p.Add("@TicketPrice", seatViewModel.TicketPrice);
                p.Add("@TransactionId", seatViewModel.TransactionId);
                p.Add("@MovieTimingId", seatViewModel.MovieTimingId);
                p.Add("@TicketCount", seatViewModel.TicketCount);
                p.Add("@PaymentType", seatViewModel.PaymentType);
                p.Add("@PaymentConfirmationNumber", seatViewModel.PaymentConfirmationNumber);
                p.Add("@IPAddress", seatViewModel.IPAddress);
                p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@OrderId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                const string storedprocedure = "dbo.mt_ins_TempOrders";
                con.Query<int>(storedprocedure, p, commandType: CommandType.StoredProcedure).SingleOrDefault();
                var result = p.Get<int>("@Output");
                var OrderId = p.Get<int>("@OrderId");

                //Error occured or validation when result value is negative.
                if (result < 0)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    return Convert.ToInt32(OrderId);
                }
            }
            catch (Exception ex)
            {
                return -101;
                throw ex;
            }
        }

        public bool CancelOrder(int orderId)
        {
            con = new SqlConnection(connectionString);

            var p = new DynamicParameters();
            p.Add("@Output", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@orderId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            const string storedprocedure = "[dbo].[mt_CancelOrder]";
            con.Query<int>(storedprocedure, p, commandType: CommandType.StoredProcedure).SingleOrDefault();
            var result = p.Get<int>("@Output");

            return result > 0;
        }
    }
}