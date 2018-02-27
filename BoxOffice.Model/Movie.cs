using System;

namespace BoxOffice.Model
{
    public class Movie
    {
        public int Id { get; set; }
        public int FK_BOTheatre_ID { get; set; }
        public string MovieName { get; set; }
        public int FK_WebMovie_ID { get; set; }
        public string MobileThumbnailImage { get; set; }
        public string ShortName { get; set; }
        public int Duration { get; set; }
        public string TicketDisplayName { get; set; }
        public int FK_Languages_ID { get; set; }
        public int FK_MovieSlab_ID { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Distributor { get; set; }

        public DateTime SHOWDATETIME { get; set; }
        public string S_Date { get; set; }
    }
}
