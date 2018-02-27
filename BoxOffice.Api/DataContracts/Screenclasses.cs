using System.Collections.Generic;
using System.Runtime.Serialization;
namespace BoxOffice.Api.DataContracts
{
    [DataContract]
    public class Screenclasses
    {
        [DataMember]
        public int ScreenClassId { get; set; }

        [DataMember]
        public string ScreenClassName { get; set; }

        [DataMember]
        public decimal TicketPrice { get; set; }

        [DataMember]
        public int RowCount { get; set; }

        [DataMember]
        public int ColumnCount { get; set; }

        [DataMember]
        public decimal CGST { get; set; }

        [DataMember]
        public decimal SGST { get; set; }

        [DataMember]
        public decimal MC { get; set; }

        [DataMember]
        public int DisplayOrder { get; set; }

        [DataMember]
        public int ScreenId { get; set; }

        [DataMember]
        public List<Seat> seats { get; set; }

        [DataMember]
        public int MovieTimingId { get; set; }
    }


}
