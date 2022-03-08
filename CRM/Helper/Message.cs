using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CRM.Helper
{
    public class Message
    {
        [DataMember]
        public bool status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
        [JsonIgnore]
        public string errors { get; set; }

        
    }
}
