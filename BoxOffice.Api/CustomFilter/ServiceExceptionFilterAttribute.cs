using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace BoxOffice.Api.CustomFilter
{

    //public class ServiceExceptionFilterAttribute : ExceptionFilterAttribute, IExceptionFilter
    //{
    //    public override void OnException(HttpActionExecutedContext actionExecutedContext)
    //    {
    //        //Check the Exception Type

    //        if (actionExecutedContext.Exception is ServiceException)
    //        {
    //            var exception = (ServiceException)actionExecutedContext.Exception;
    //            //The Response Message Set by the Action During Ececution
    //            var res = actionExecutedContext.Exception.Message;

    //            //Define the Response Message
    //            //HttpResponseMessage response = new HttpResponseMessage(exception.HttpCode)
    //            //{
    //            //    Content = new StringContent(res),
    //            //    ReasonPhrase = res
    //            //};

    //            ////Create the Error Response
    //            //actionExecutedContext.Response = response;
    //        }
    //    }
    //}
}