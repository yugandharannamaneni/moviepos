using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BoxOffice.DAL.Interfaces;
using AutoMapper;
using log4net;
using DataContracts = BoxOffice.Api.DataContracts;
using DataLayer = BoxOffice.Model;
using BoxOffice.Api.DataContracts;

namespace BoxOffice.Api.Controllers
{
    /// <summary>
    /// Master Data API: Provides api for theatre master data.
    /// </summary>
    [RoutePrefix("api/theater")]
    [BoxOfficeAuthorize]
    public class SeatBookingController : ApiController
    {
        private ILog Log = LogManager.GetLogger(typeof(MasterDataController));

        private ISeatBookingRepository _seatBookingRepository;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="seatBookingRepository">ISeatBookingRepository</param>
        public SeatBookingController(ISeatBookingRepository seatBookingRepository)
        {
            _seatBookingRepository = seatBookingRepository;
        }

        /// <summary>
        /// Returns list of theatres available.
        /// </summary>
        /// <returns>List of Theatre</returns>
        [Route("bookseats")]
        public BookSeatsResponse CreateTempOrder(DataContracts.TempOrderViewModel tempOrder)
        {
            BookSeatsResponse response = new BookSeatsResponse();
            try
            {
                DataLayer.SeatViewModel tempOrderModel = GetTempOrder(tempOrder);

                var result = _seatBookingRepository.CreateTempOrder(tempOrderModel);

                if (result < 0)
                {
                    if (result == -1)
                    {
                        response.ErrorMessage = "There are no seats available in booking";
                        response.ErrorCode = 1;

                        return response;
                    }

                    if (result == -2)
                    {
                        response.ErrorMessage = "Requested seats are more than allowed seats per booking.";
                        response.ErrorCode = 2;

                        return response;
                    }

                    if (result == -3)
                    {
                        response.ErrorMessage = "Requested seats are not allocated to your vendor Id.";
                        response.ErrorCode = 3;

                        return response;
                    }

                    if (result == -4)
                    {
                        response.ErrorMessage = "Some of the seats or all seats already booked.";
                        response.ErrorCode = 4;

                        return response;
                    }
                    if (result == -101)
                    {
                        response.ErrorMessage = "Internal Server Error occured.";
                        response.ErrorCode = -101;
                        return response;
                    }
                }
                response.ErrorCode = 0;
                response.tempOrderId = result;
                response.ErrorMessage = "Seats are booked successfully.";

                return response;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error occured while creating temp order for ScreenId {0}: {1}", tempOrder.ScreenId, ex);
                response.ErrorMessage = "Internal Server Error occured.";
                response.ErrorCode = -101;

                return response;
            }
        }

        private DataLayer.SeatViewModel GetTempOrder(DataContracts.TempOrderViewModel tempOrder)
        {
            DataLayer.SeatViewModel tempOrderModel = new DataLayer.SeatViewModel();
            tempOrderModel.ScreenId = tempOrder.ScreenId;
            tempOrderModel.ScreenClassId = tempOrder.ScreenClassId;
            tempOrderModel.MovieTimingId = tempOrder.MovieTimingId;
            tempOrderModel.VendorId = tempOrder.VendorId;
            tempOrderModel.TicketCount = tempOrder.TicketCount;
            tempOrderModel.IPAddress = tempOrder.IPAddress;
            tempOrderModel.TotalAmount = tempOrder.TotalPrice;
            tempOrderModel.TicketPrice = tempOrder.TicketPrice;
            tempOrderModel.WebOrderId = tempOrder.WebOrderId;
            tempOrderModel.SeatIds = string.Join(",", tempOrder.Seats.Select(seat => seat.ID));

            return tempOrderModel;
        }

        /// <summary>
        /// Cancel Temp Order Id based on temp Order Id passed.
        /// </summary>
        /// <param name="tempOrderId"></param>
        /// <returns></returns>
        [Route("bookseats/cancel")]
        public bool CancelTempOrder(int tempOrderId)
        {
            try
            {
                if (tempOrderId == 0)
                    return false;

                return _seatBookingRepository.CancelTempOrder(tempOrderId);
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error occured while cancel temp order with Order Id {0}: {1}", tempOrderId, ex);
                return false;
            }
        }

        [Route("buyseats")]
        public BuySeatsResponse CreateOrder(DataContracts.OrderViewModel order)
        {
            BuySeatsResponse response = new BuySeatsResponse();
            try
            {
                DataLayer.SeatViewModel orderModel = GetOrder(order);

                var result = _seatBookingRepository.CreateOrder(orderModel);

                if (result < 0)
                {
                    if (result == -1)
                    {
                        response.ErrorMessage = "Blocked seats with this booking has been expired. Please try to book again.";
                        response.ErrorCode = 1;

                        return response;
                    }

                    if (result == -2)
                    {
                        response.ErrorMessage = "Requested seats are more than allowed seats per booking.";
                        response.ErrorCode = 2;

                        return response;
                    }

                    if (result == -3)
                    {
                        response.ErrorMessage = "Requested seats are not allocated to your vendor Id.";
                        response.ErrorCode = 3;

                        return response;
                    }

                    if (result == -4)
                    {
                        response.ErrorMessage = "Some of the seats or all seats already booked.";
                        response.ErrorCode = 4;

                        return response;
                    }
                    if (result == -101)
                    {
                        response.ErrorMessage = "Internal Server Error occured.";
                        response.ErrorCode = -101;
                        return response;
                    }
                }
                response.ErrorCode = 0;
                response.OrderId = result;
                response.ErrorMessage = "Order created successfully.";

                return response;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error occured while creating order for ScreenId {0}, tempOrder: {1}, exception Details: {2}", order.ScreenId, order.TempOrderId, ex);
                response.ErrorMessage = "Internal Server Error occured.";
                response.ErrorCode = -101;

                return response;
            }
        }

        private DataLayer.SeatViewModel GetOrder(DataContracts.OrderViewModel order)
        {
            DataLayer.SeatViewModel orderModel = new DataLayer.SeatViewModel();
            orderModel.ScreenId = order.ScreenId;
            orderModel.ScreenClassId = order.ScreenClassId;
            orderModel.MovieTimingId = order.MovieTimingId;
            orderModel.VendorId = order.VendorId;
            orderModel.TicketCount = order.TicketCount;
            orderModel.IPAddress = order.IPAddress;
            orderModel.TotalAmount = order.TotalPrice;
            orderModel.TicketPrice = order.TicketPrice;
            orderModel.WebOrderId = order.WebOrderId;
            orderModel.SeatIds = string.Join(",", order.Seats.Select(seat => seat.ID));
            orderModel.TempOrderId = order.TempOrderId;
            orderModel.MobileNumber = order.MobileNumber;
            orderModel.ShowDate = order.ShowDate;
            orderModel.ShowTime = order.ShowTime;
            orderModel.TransactionId = order.TransactionId;
            orderModel.PaymentType = order.PaymentType;
            orderModel.PaymentConfirmationNumber = order.PaymentConfirmationNumber;

            return orderModel;
        }

        /// <summary>
        /// Cancel Order Id based on Order Id passed.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("buyseats/cancel")]
        public bool CancelOrder(int orderId)
        {
            try
            {
                if (orderId == 0)
                    return false;

                return _seatBookingRepository.CancelOrder(orderId);
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error occured while cancel temp order with Order Id {0}: {1}", orderId, ex);
                return false;
            }
        }
    }
}