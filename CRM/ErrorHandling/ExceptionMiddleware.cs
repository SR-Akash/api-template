using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CRM.ErrorHandling
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;


        public ExceptionMiddleware(RequestDelegate next)
        {

            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // return context.Response.WriteAsync(new ErrorDetails()
            // {
            //     statuscode = context.Response.StatusCode,
            //     message = exception.Message //"Internal Server Error from the custom middleware."
            // }.ToString());
            // JsonConvert.SerializeObject(

            var error = new ErrorDetails()
            {
                statuscode = context.Response.StatusCode,
                message = exception.Message //"Internal Server Error from the custom middleware."
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(error));
        }
    }
}
