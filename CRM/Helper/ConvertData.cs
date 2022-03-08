using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Helper
{
    public static class ConvertData
    {
        public static string ToJSON(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
