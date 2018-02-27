using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace BoxOffice.Api.DataContracts
{
    [DataContract(Name = "SeatBooking")]
    public class TempOrderViewModel
    {
        [DataMember(Name = "movieTimingId")]
        public int MovieTimingId { get; set; }

        [DataMember(Name = "screenId")]
        public int ScreenId { get; set; }

        [DataMember(Name = "screenClassId")]
        public int ScreenClassId { get; set; }

        [DataMember(Name = "vendorId")]
        public int VendorId { get; set; }

        [DataMember(Name = "ticketCount")]
        public int TicketCount { get; set; }

        [DataMember(Name = "ipAddress")]
        public string IPAddress { get; set; }

        [DataMember(Name ="ratePerTicket")]
        public decimal TicketPrice { get; set; }

        [DataMember(Name = "totalPrice")]
        public decimal TotalPrice { get; set; }

        [DataMember(Name = "webBookingId")]
        public int WebOrderId { get; set; }

        [DataMember(Name = "seats")]
        public List<Seat> Seats { get; set; }
    }

    [DataContract(Name = "order")]
    public class OrderViewModel
    {
        [DataMember(Name = "movieTimingId")]
        public int MovieTimingId { get; set; }

        [DataMember(Name = "screenId")]
        public int ScreenId { get; set; }

        [DataMember(Name = "screenClassId")]
        public int ScreenClassId { get; set; }

        [DataMember(Name = "vendorId")]
        public int VendorId { get; set; }

        [DataMember(Name = "ticketCount")]
        public int TicketCount { get; set; }

        [DataMember(Name = "ipAddress")]
        public string IPAddress { get; set; }

        [DataMember(Name = "ratePerTicket")]
        public decimal TicketPrice { get; set; }

        [DataMember(Name = "totalPrice")]
        public decimal TotalPrice { get; set; }

        [DataMember(Name = "webBookingId")]
        public int WebOrderId { get; set; }

        [DataMember(Name = "seats")]
        public List<Seat> Seats { get; set; }

        [DataMember(Name = "tempOrderId")]
        public int TempOrderId { get; set; }

        [DataMember(Name = "mobileNumber")]
        public string MobileNumber { get; set; }

        [DataMember(Name = "showDate")]
        public DateTime ShowDate { get; set; }

        [DataMember(Name = "showTime")]
        public DateTime ShowTime { get; set; }

        [DataMember(Name = "transactionId")]
        public string TransactionId { get; set; }

        [DataMember(Name = "paymentType")]
        public string PaymentType { get; set; }

        [DataMember(Name = "paymentConfirmationNumber")]
        public string PaymentConfirmationNumber { get; set; }
    }
}