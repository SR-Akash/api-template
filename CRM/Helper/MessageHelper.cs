using System.Runtime.Serialization;

namespace CRM.Helper
{
    public class MessageHelper
    {
         
        public string Message { get; set; }
        public int statuscode { get; set; }
        public long Key { get; set; }
        public string InvoiceCode { get; set; }
        public msg data { get; set; }

    }

    public class msg
    {

        public string Message { get; set; }
        public int statuscode { get; set; }
        public long Key { get; set; }
        public string InvoiceCode { get; set; }
    }
}
