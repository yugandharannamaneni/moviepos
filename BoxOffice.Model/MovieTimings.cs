using System;
using System.Collections.Generic;

namespace BoxOffice.Model
{
    public class MovieTimings
    {
        public int Id { get; set; }
        public int MovieTimingId { get; set; }
        public string MovieName { get; set; }
        public string MovieTime { get; set; }
        public DateTime ShowDateTime { get; set; }
        public DateTime Date { get; set; }
        public int ScreenId { get; set; }
        public int MovieId { get; set; }
        public bool IsOnline { get; set; }
        public bool LiveSharing { get; set; }
        public List<Screenclasses> scs { get; set; }
        public string TemplateIds { get; set; }
        public int FK_WEBMOIVE_ID { get; set; }

        public string Movie_Timings { get; set; }
        public int Days { get; set; }

        public List<TicketPricesAndTaxes> CustomTicketPrices { get; set; }
    }

    public class MovieTimingForSync
    {
        public int Id { get; set; }
        public string ScreenId { get; set; }
        public string ShowDateTime { get; set; }
        public DateTime ShowDate { get; set; }
        public DateTime WebMovieId { get; set; }
        public int InternetSales { get; set; }
        public int OnlineSalesDate { get; set; }
        public bool IsActive { get; set; }
        public bool ScheduleUploaded { get; set; }
        public bool IsDirty { get; set; }
        public bool IsReported { get; set; }
        public bool IsClosedOnline { get; set; }
        public bool IsLocallyReported { get; set; }
        public int WebMovieTimingId { get; set; }
        public int WebScreenId { get; set; }

        public List<TicketPricesAndTaxes> CustomTicketPrices { get; set; }
    }
}