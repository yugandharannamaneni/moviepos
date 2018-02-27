using System.Runtime.Serialization;
namespace BoxOffice.Api.DataContracts
{
    [DataContract]
    public class Seat
    {
        [DataMember(Name = "SeatId")]
        public int ID { get; set; }

        [DataMember]
        public int ScreenClassId { get; set; }

        [DataMember(Name = "SeatStatus")]
        public int Status { get; set; }

        [DataMember]
        public int RowValue { get; set; }

        [DataMember]
        public int ColumnValue { get; set; }

        [DataMember]
        public string RowText { get; set; }

        [DataMember]
        public string ColumnText { get; set; }

        [DataMember]
        public int DisplayOrder { get; set; }

        [DataMember]
        public string ScreenClass { get; set; }

        [DataMember(Name = "ScreenId")]
        public int Screen_Id { get; set; }

        [DataMember]
        public float Price { get; set; }
    }
}