using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CRM.Helper
{
    public class TimeZoneDates
    {
        public static DateTime GetCurrentDatTime()
        {
            TimeZoneInfo timeZoneInfo;
            DateTime dateTime = DateTime.UtcNow;

            // TimeZoneInfo easternStandardTime;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
                dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                return dateTime;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Asia/Dhaka");
                dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                return dateTime;
            }
            //else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            //{
            //    return dateTime;
            //}
            else
            {
                return dateTime;
            }
        }
    }
}
