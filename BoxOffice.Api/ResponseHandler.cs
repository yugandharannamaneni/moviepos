using BoxOffice.Api.CustomFilter;
using BoxOffice.Api.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BoxOffice.Api
{
    public class ResponseHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            return BuildApiResponse(request, response);
        }

        private static HttpResponseMessage BuildApiResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            object content;
            string errorMessage = null;

            if (request.RequestUri.AbsolutePath.ToLowerInvariant().Contains("swagger"))
            {
                return response;
            }


            if (response.TryGetContentValue(out content) && !response.IsSuccessStatusCode)
            {
                ServiceException error = null;
                if (content.GetType() == typeof(ServiceException))
                    error = content as ServiceException;

                if (error != null)
                {
                    content = null;
                    errorMessage = error.Message;

#if DEBUG
                    errorMessage = string.Concat(errorMessage, error.ErrorCode, error.StackTrace);

                    var newResponse1 = request.CreateResponse(error.HttpCode, new BaseResponse(error.HttpCode, content, errorMessage));

                    foreach (var header in response.Headers)
                    {
                        newResponse1.Headers.Add(header.Key, header.Value);
                    }
                    return newResponse1;
#endif
                }
            }

            var newResponse = request.CreateResponse(response.StatusCode, new BaseResponse(response.StatusCode, content, errorMessage));

            foreach (var header in response.Headers)
            {
                newResponse.Headers.Add(header.Key, header.Value);
            }

            return newResponse;
        }
    }
}