using System.Collections.Generic;
namespace BoxOffice.Model
{
    public class Screenclasses
    {
        public int ScreenClassId { get; set; }
        public string ScreenClassName { get; set; }
        public decimal TicketPrice { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal MC { get; set; }
        public int DisplayOrder { get; set; }
        public int ScreenId { get; set; }
        public List<Seat> seats { get; set; }
        public int MovieTimingId { get; set; }
    }


}
