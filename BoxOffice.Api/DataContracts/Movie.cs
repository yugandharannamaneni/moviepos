using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BoxOffice.Api.DataContracts
{
    [DataContract]
    public class Movie
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int TheatreId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ShortName { get; set; }

        [DataMember]
        public string Duration { get; set; }

        [DataMember]
        public string TicketDisplayName { get; set; }

        [DataMember]
        public string MobileThumbnailImage { get; set; }

        [DataMember]
        public string ReleaseDate { get; set; }

        [DataMember]
        public string Distributor { get; set; }    
    }
}