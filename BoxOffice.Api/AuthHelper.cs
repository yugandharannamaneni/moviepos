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
    public static class AuthHelper
    {
        private static string secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk"; // This secret key is configurable
        public static string CreateJSONWebToken(string userId, string password, string vendor)
        {
            var now = Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
            var exp = Math.Round((DateTime.UtcNow.AddMinutes(30) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
            var payload = new Dictionary<string, object>() {
                {"id",userId},
                {"password",password},
                {"vendor",vendor},
                {"iat",now},
                {"exp",exp}
            };

            const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, secretKey);

            return token;
        }

        public static string DecodeJSONWebToken(string token)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                return decoder.Decode(token, secretKey, verify: true);
            }
            catch (JWT.SignatureVerificationException)
            {
                return "Invalid Token";
            }
        }
    }

    public class PayLoad
    {
        public string id { get; set; }
        public string password { get; set; }
        public string vendor { get; set; }
        public string iat { get; set; }
        public string exp { get; set; }
    }

    public class JSONToken
    {
        public string token { get; set; }
    }
}