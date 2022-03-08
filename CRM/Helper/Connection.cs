using System.IO;
using Microsoft.Extensions.Configuration;

namespace CRM.Helper
{
    public class Connection
    {  
        public static string iDefaults { get; internal set; }
        private static string GetConnection()
        {
            var config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetDirectoryRoot(@"\"))
           .AddJsonFile("appSettings.json", false)
           .Build();
            var connString = config.GetSection("connectionStrings").Value;
            return connString;
        }
    }
}
