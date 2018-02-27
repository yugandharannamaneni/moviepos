using System.Runtime.Serialization;
namespace BoxOffice.Api.DataContracts
{
    [DataContract]
    public class Screen
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string ScreenName { get; set; }

        [DataMember]
        public string PriceCards { get; set; }

        [DataMember]
        public int Capacity { get; set; }
    }
}
