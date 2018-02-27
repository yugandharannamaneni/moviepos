using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;

namespace BoxOffice.Api
{
    public class BoxOfficeAuthorize : System.Web.Http.AuthorizeAttribute
    {
        public override void OnAuthorization(
               System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!IsAuthorized(actionContext))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid authentication token");
            }
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Contains("Auth-Token") && actionContext.Request.Headers.GetValues("Auth-Token") != null)
            {
                // get value from header
                string authToken = Convert.ToString(actionContext.Request.Headers.GetValues("Auth-Token").FirstOrDefault());

                var payload = AuthHelper.DecodeJSONWebToken(authToken);
                var payLoadInfo = JsonConvert.DeserializeObject<PayLoad>(payload);

                if (payLoadInfo.vendor.ToLower() == "bookmyshow")
                {
                    if (payLoadInfo.id.ToLower() == ConfigurationManager.AppSettings["bmsUserName"]
                        && payLoadInfo.password.ToLower() == ConfigurationManager.AppSettings["bmsPassword"])
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
            //return base.IsAuthorized(actionContext);
        }
    }
}