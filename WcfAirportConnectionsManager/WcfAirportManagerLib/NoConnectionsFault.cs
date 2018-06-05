using System.Runtime.Serialization;

namespace WcfAirportManagerLib
{
    [DataContract]
    public class NoConnectionsFault
    {
        [DataMember]
        public bool Result { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
