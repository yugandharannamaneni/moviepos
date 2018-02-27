using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;

namespace BoxOffice.Api.DataContracts
{
    [DataContract]
    public class BaseResponse
    {
        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public int StatusCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ErrorMessage { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public object Data { get; set; }

        public BaseResponse(HttpStatusCode statusCode, object result = null, string errorMessage = null)
        {
            Version = "1.0";
            StatusCode = (int)statusCode;
            Data = result;
            ErrorMessage = errorMessage;
        }
    }
}