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
using System.Text;

namespace CRM.middleware
{
    public class ResponseRewindMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IHostEnvironment _env;
        public IConfiguration Configuration { get; }
        public ResponseRewindMiddleware(RequestDelegate next, IConfiguration configuration, IHostEnvironment env)
        {
            this.next = next;
            _env = env;
            Configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {

            Stream originalBody = context.Response.Body;

            try
            {
                var audienceConfig = Configuration.GetSection("Audience");
                string key = audienceConfig["sec"].Trim();
                string iv = audienceConfig["sec"].Trim();
                using (var memStream = new MemoryStream())
                {
                    context.Response.Body = memStream;

                    await next(context);

                    memStream.Position = 0;
                    string responseBody = new StreamReader(memStream).ReadToEnd();
                   string response= AesOperation.EncryptionString(responseBody, key, iv);
                    byte[] data = Encoding.UTF8.GetBytes(response);

                    memStream.Write(data, 0, data.Length);
                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody);
                }

            }
            finally
            {
                context.Response.Body = originalBody;
            }

        }
    }
}
