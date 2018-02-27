namespace BoxOffice.Model
{
    public class Seat
    {
        public int ID { get; set; }
        public int ScreenClassId { get; set; }
        public int FK_SeatStatus_ID { get; set; }
        public int RowValue { get; set; }
        public int ColumnValue { get; set; }
        public string RowText { get; set; }
        public string ColumnText { get; set; }
        public bool IsHeldforInternet { get; set; }
        public int SeatTemplateNameId { get; set; }
        public int DisplayOrder { get; set; }
        public int VendorId { get; set; }
        public int IsReserved { get; set; }
        public string Color { get; set; }
        public string ScreenClass { get; set; }
        public int Screen_Id { get; set; }
        public int Status { get; set; }

        public bool Is_Enabled { get; set; }
        public double Price { get; set; }
    }
}