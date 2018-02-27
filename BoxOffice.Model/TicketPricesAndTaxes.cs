namespace BoxOffice.Model
{
    public class TicketPricesAndTaxes
    {
        public int Id { get; set; }
        public int FK_MovieTimings_ID { get; set; }
        public string ClassName { get; set; }
        public string FK_ScreenClasses_ID { get; set; }
        public decimal TicketPrice { get; set; }
        public int TaxConstantValue { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal MC { get; set; }
        public decimal TaxPercentage { get; set; }
    }
}
