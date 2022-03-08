using System.Collections.Generic;
using System.Text;

namespace CRM.Helper
{
    public class CommonHelper
    {
        public static string ConcateStringWithComma(List<string> ObjList)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string? s in ObjList)
            {
                if (s != null && s != "")
                    builder.Append(s).Append(", ");
            }
            return builder.ToString().TrimEnd(new char[] { ',', ' ' });
        }
    }
}
