using System;

namespace BoxOffice.Model
{
    public class SeatViewModel
    {
        public int ScreenId { get; set; }
        public int ScreenClassId { get; set; }
        public int MovieTimingId { get; set; }
        public string SeatIds { get; set; }
        public int VendorId { get; set; }
        public int TicketCount { get; set; }
        public string IPAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TicketPrice { get; set; }
        public int WebOrderId { get; set; }
        public int TempOrderId { get; set; }
        public string MobileNumber { get; set; }
        public DateTime ShowDate { get; set; }
        public DateTime ShowTime { get; set; }
        public string TransactionId { get; set; }
        public string PaymentType { get; set; }
        public string PaymentConfirmationNumber { get; set; }
    }
}
