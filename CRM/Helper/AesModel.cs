using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Helper
{
    public  class AesModel
    {
        private readonly IHostEnvironment _env;
        public IConfiguration Configuration { get; }

        public AesModel(IConfiguration configuration, IHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;

        }

        public async Task<dynamic> Encription(string data)
        {
            var audienceConfig = Configuration.GetSection("Audience");
            string key = audienceConfig["sec"].Trim();
            string iv = audienceConfig["sec"].Trim();
            
            if (_env.IsDevelopment())
            {
                return await Task.FromResult(AesOperation.EncryptionString(data, key, iv));
            }
            else
            {
                return await Task.FromResult(data);
            }
           
            
        }
        public async Task<dynamic> DecryptString(string data)
        {
            var audienceConfig = Configuration.GetSection("Audience");
            string key = audienceConfig["sec"].Trim();
            string iv = audienceConfig["sec"].Trim();
            if (_env.IsProduction())
            {
                return await Task.FromResult(AesOperation.DecryptString(key,data));
            }
            else
            {
                return await Task.FromResult(data);
            }
             
        }
    }
}
