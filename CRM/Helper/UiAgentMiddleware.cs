using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace CRM.Helper
{
    public class UiAgentMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;


        public UiAgentMiddleware(RequestDelegate next, IConfiguration configuration)
        {

            _configuration = configuration;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            var Agent = httpContext.Request.Headers["User-Agent"].ToString();

            UiAgent.CrossCheck(Agent.ToLower());

            await _next(httpContext);
            await httpContext.Request.Body.DisposeAsync();
            await httpContext.Response.Body.DisposeAsync();
        }
    }
}
