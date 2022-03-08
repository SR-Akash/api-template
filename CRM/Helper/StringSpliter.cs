
using System.Collections.Generic;
using System.Text;


namespace CRM.Helper
{
    public class StringSpliter
    {
        public static string ByPascaleCase(string str)
        {
            Prefix().ForEach(y =>
            {
                if (str.StartsWith(y))
                    str = (str.Remove(0, y.Length));
            });

            var strBuild = new StringBuilder();

            foreach (var c in str)
            {
                if (char.IsUpper(c))
                    strBuild.Append(' ');

                strBuild.Append(c);
            }

            return strBuild.ToString().Trim();
        }
        private static List<string> Prefix()
        {
            return new List<string> { "str", "int", "dte", "num", "mon" };
        }
    }
}
