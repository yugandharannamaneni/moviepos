using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;

namespace BoxOffice.Api.CustomFilter
{
    public class ServiceException : Exception
    {
        public HttpStatusCode HttpCode
        {
            get { return HttpStatusCode.OK; }
        }

        public int ErrorCode
        {
            get { return 0; }
        }

        public string ErrorMessage
        {
            get { return string.Empty; }
        }

        public IReadOnlyDictionary<string, string> ErrorValues
        {
            get { return null; }
        }

        public ServiceException(HttpStatusCode httpCode = default(HttpStatusCode), string message = null, Exception innerException = null, int errorCode = 0, string errorMessage = null, IReadOnlyDictionary<string, string> errorValues = null)
            : base(message, innerException)
        {
            //HttpCode = httpCode;
            //ErrorCode = errorCode;
            //ErrorMessage = errorMessage;
            //ErrorValues = errorValues ?? new Dictionary<string, string>(); // Can't use ImmutableDictionary.Empty, it's not serializeable
        }

        private ServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

            //HttpCode = (HttpStatusCode)info.GetInt32("HttpCode");

            //ErrorCode = info.GetInt32("ErrorCode");
            //ErrorMessage = info.GetString("ErrorMessage");

            //ErrorValues = (IReadOnlyDictionary<string, string>)info.GetValue("ErrorValues", typeof(IReadOnlyDictionary<string, string>));
        }


        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("HttpCode", (int)HttpCode);
            info.AddValue("ErrorCode", ErrorCode);
            info.AddValue("ErrorMessage", ErrorMessage);
            info.AddValue("ErrorValues", ErrorValues, typeof(IReadOnlyDictionary<string, string>));
        }


        public static ServiceException Wrap(HttpStatusCode httpCode, string message, params object[] messageArgs)
        {
            message = messageArgs.Length > 0 ? string.Format(message, messageArgs) : message;

            return new ServiceException(httpCode, message, null, 100, "test", null);

        }
    }
}

