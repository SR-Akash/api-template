using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Helper;
using System.IO;
using System.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace CRM.middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class EncryptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public IConfiguration Configuration { get; }

        public EncryptionMiddleware(RequestDelegate next, IConfiguration configuration, IHostEnvironment env)
        {
            _next = next;
            _env = env;
            Configuration = configuration;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();
             
            var api = new ApiRequestInputViewModel
                {
                    HttpType = httpContext.Request.Method,
                    Query = httpContext.Request.QueryString.Value,
                    RequestUrl = httpContext.Request.Path,
                    RequestName = "",
                    RequestIP = httpContext.Request.Host.Value
                };

                var request = httpContext.Request.Body;
                var response = httpContext.Response.Body;
                var audienceConfig = Configuration.GetSection("Audience");
                string key = audienceConfig["sec"].Trim();
                string iv = audienceConfig["sec"].Trim();
                try
                {
                    using (var newRequest = new MemoryStream())
                    {
                        //request
                        httpContext.Request.Body = newRequest;

                        using (var newResponse = new MemoryStream())
                        {
                            //response
                            httpContext.Response.Body = newResponse;

                            using (var reader = new StreamReader(request))
                            {

                                api.Body = await reader.ReadToEndAsync();
                                if (string.IsNullOrEmpty(api.Body))
                                    await _next.Invoke(httpContext);

                                api.Body = AesOperation.DecryptString(key, api.Body);
                            }
                            using (var writer = new StreamWriter(newRequest))
                            {
                                await writer.WriteAsync(api.Body);
                                await writer.FlushAsync();
                                newRequest.Position = 0;
                                httpContext.Request.Body = newRequest;
                                await _next(httpContext);
                            }

                            using (var reader = new StreamReader(newResponse))
                            {
                                newResponse.Position = 0;
                                api.ResponseBody = await reader.ReadToEndAsync();
                                if (!string.IsNullOrWhiteSpace(api.ResponseBody))
                                {


                                    // byte[] data = Encoding.UTF8.GetBytes(AesOperation.DiscountString(responseBody, key, iv));

                                    api.ResponseBody = AesOperation.EncryptionString(api.ResponseBody, key, iv);
                                }
                            }
                            using (var writer = new StreamWriter(response))
                            {
                                await writer.WriteAsync(api.ResponseBody);
                                await writer.FlushAsync();
                            response.Position = 0;
                            httpContext.Response.Body = response;
                            await _next(httpContext);
                           
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //using (var writer = new StreamWriter(response))
                    //{
                    //    await writer.WriteAsync("Please contact with system administrator");
                    //    await writer.FlushAsync();
                    //}
                }
                finally
                {
                    httpContext.Request.Body = request;
                    httpContext.Response.Body = response;
                }
             
            
            

            

             
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class EncryptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseEncryptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EncryptionMiddleware>();
        }
    }
}
