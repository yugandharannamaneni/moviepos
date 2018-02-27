using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BoxOffice.Api.DataContracts
{
    [DataContract]
    public class MovieTimings
    {
        [DataMember]
        public int Id { get; set; }
        
        [DataMember]
        public string MovieName { get; set; }

        [DataMember]
        public string MovieTime { get; set; }

        [DataMember]
        public DateTime ShowDateTime { get; set; }

        [DataMember(Name="ShowDate")]
        public DateTime Date { get; set; }

        [DataMember]
        public int ScreenId { get; set; }

        [DataMember]
        public int MovieId { get; set; }

        [DataMember(Name="ScreenClasses")]
        public List<Screenclasses> scs { get; set; }
    }
}