using BoxOffice.Api.CustomFilter;
using BoxOffice.DAL.Interfaces;
using log4net;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BoxOffice.Api.Controllers
{
    /// <summary>
    /// Master Data API: Provides api for theatre master data.
    /// </summary>
    [RoutePrefix("api/login")]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
    public class LoginController : ApiController
    {
        private ILog Log = LogManager.GetLogger(typeof(MasterDataController));

        private ISeatTemplateRepository _seatTemplateRepository;
        private ISeatLayoutConfig _seatLayoutConfig;
        private IMovieTimingsRepository _movieTimingsRepository;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="seatTemplateRepository">ISeatTemplateRepository</param>
        /// <param name="seatLayoutConfig">ISeatLayoutConfig</param>
        /// <param name="movieTimingsRepository">IMovieTimingsRepository</param>
        public LoginController(ISeatTemplateRepository seatTemplateRepository,
            ISeatLayoutConfig seatLayoutConfig,
            IMovieTimingsRepository movieTimingsRepository)
        {
            _seatTemplateRepository = seatTemplateRepository;
            _seatLayoutConfig = seatLayoutConfig;
            _movieTimingsRepository = movieTimingsRepository;
        }

        public class User
        {
            public string username { get; set; }
            public string password { get; set; }
            public string vendorname { get; set; }
        }

        /// <summary>
        /// Returns list of theatres available.
        /// </summary>
        /// <returns>List of Theatre</returns>
        [HttpPost]
        public HttpResponseMessage Login(User user)
        {
            try
            {
                if (user.vendorname.ToLower() == "bookmyshow")
                {
                    if (user.username.ToLower() == ConfigurationManager.AppSettings["bmsUserName"]
                        && user.password.ToLower() == ConfigurationManager.AppSettings["bmsPassword"])
                    {
                        var jsonToken = new JSONToken();
                        jsonToken.token = AuthHelper.CreateJSONWebToken(user.username, user.password, user.vendorname);
                        return ConstructedJSONToken(jsonToken, user.username);
                    }
                    return Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid username or password");
                }
                return Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid vendor name");
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error occured while getting theatre list: ", ex);
                throw new ServiceException(message: "Internal Server Error occured while processing the request", httpCode: HttpStatusCode.InternalServerError);
            }
        }

        private HttpResponseMessage ConstructedJSONToken(JSONToken jsonToken, params string[] responseParams)
        {
            var json = JsonConvert.SerializeObject(jsonToken);
            var response = Request.CreateResponse(HttpStatusCode.OK, "token");

            response = AddCustomDataToHttpResponseHeader(response, responseParams);

            response.Content = new StringContent(json, Encoding.Unicode);
            return response;
        }

        private HttpResponseMessage AddCustomDataToHttpResponseHeader(HttpResponseMessage response, params string[] responseParams)
        {
            if (responseParams.Length <= 0) return response;

            foreach (var parameter in responseParams)
            {
                var splitParams = parameter.Split(':');

                if (splitParams.Length != 2) continue;
                var propertyName = splitParams[0];
                var propertyValue = splitParams[1];

                response.Headers.Add(propertyName, propertyValue);
            }
            return response;
        }
    }
}