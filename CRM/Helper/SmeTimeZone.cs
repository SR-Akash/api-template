using System;
using System.Runtime.InteropServices;

namespace CRM.Helper
{
    public class CRMTimeZone
    {
        public static DateTime CurrentDateTime()
        {
            TimeZoneInfo timeZoneInfo;
            DateTime dateTime;
            //Set the time zone information to US Mountain Standard Time 

            //timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");

            //Get date and time in US Mountain Standard Time 
            //  dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            //Print out the date and time

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
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                throw new Exception("I don't know how to do a lookup on a Mac TimeZone");
            }
            else
            {
                throw new Exception("I don't know how to do a lookup TimeZone");
            }
        }

    }
}
