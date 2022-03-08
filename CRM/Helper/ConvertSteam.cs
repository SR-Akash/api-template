using System.IO;
using System.Text;

namespace CRM.Helper
{
    public static class ConvertSteam
    {
        public static Stream ToStream(this string value, Encoding encoding = null)
        => new MemoryStream((encoding ?? Encoding.UTF8).GetBytes(value ?? string.Empty));
    }
}