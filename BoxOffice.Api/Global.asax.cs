using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Security;

namespace BoxOffice.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutofacConfig.ConfigureContainer();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperWebConfiguration.Configure();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var path = Request.RawUrl.ToLower();
            var agent = Request.UserAgent;

            //if (path.IndexOf("/api/paymentsuccess/paymentconfirmed") > -1 || path.IndexOf("/api/qrcode/get") > -1) return;

            if (!path.StartsWith("/api/")) return;
            if (path.Contains("login")) return;
            if (Request.Headers.AllKeys.Contains("Auth-Token"))
            {
                var authToken = Request.Headers["Auth-Token"];
                if (authToken == ConfigurationManager.AppSettings["authtoken"]) return;
                Response.StatusCode = 404;
            }
            else
            {
                Response.StatusCode = 404;
            }
            if (FormsAuthentication.RequireSSL && !Request.IsSecureConnection)
            {
                Response.Redirect(Request.Url.AbsoluteUri.Replace("http://", "https://"));
            }
        }
    }
}
