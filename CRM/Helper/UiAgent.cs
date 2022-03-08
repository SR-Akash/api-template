using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Helper
{
    public class UiAgent
    {
        public static void CrossCheck(string AgentName)
        {
            CrossPlatform().ForEach(x =>
            {
                if (AgentName.Contains(x))
                    throw new Exception("Unauthorize Client Detected!");
            });

        }

        // Add Blocking Server Name
        private static List<string> CrossPlatform()
        {
            return new List<string>
            {
                "postman","curl","apache"
            };
        }
    }
}
