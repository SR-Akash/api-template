using System.Collections.Generic;

namespace CRM.Helper
{
    public class MonthCollection
    {
        public long Value { get; set; }
        public string Name { get; set; }

        public static List<MonthCollection> GetMonthList()
        {
            return new List<MonthCollection> {
                new MonthCollection { Value = 01, Name = "January" },
                new MonthCollection { Value = 02, Name = "February" },
                new MonthCollection { Value = 03, Name = "March" },
                new MonthCollection { Value = 04, Name = "April" },
                new MonthCollection { Value = 05, Name = "May" },
                new MonthCollection { Value = 06, Name = "June" },
                new MonthCollection { Value = 07, Name = "July" },
                new MonthCollection { Value = 08, Name = "August" },
                new MonthCollection { Value = 09, Name = "September" },
                new MonthCollection { Value = 10, Name = "October" },
                new MonthCollection { Value = 11, Name = "November" },
                new MonthCollection { Value = 12, Name = "December" },
            };
        }
    }
}