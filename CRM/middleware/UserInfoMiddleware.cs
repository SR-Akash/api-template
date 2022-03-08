using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using CRM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class UserInfoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public IConfiguration Configuration { get; }
        public UserInfoMiddleware(RequestDelegate next, IConfiguration configuration, IHostEnvironment env)
        {
            _next = next;
            _env = env;
            Configuration = configuration;
        }

        public Task Invoke(HttpContext httpContext)
        {
            try
            {
                if (_env.IsProduction())
                {
                    var email = httpContext.User.Identity.GetId("emailaddress");
                    UserInfo.EmailId = email;
                }
                else
                {

                }
                
            }
            catch { }
           


            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class UserInfoMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserInfoMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserInfoMiddleware>();
        }
    }
}
