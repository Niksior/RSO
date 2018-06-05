using System.Runtime.Serialization;

namespace WcfAirportManagerLib
{
    [DataContract]
    public class InvalidInputFault
    {
        [DataMember]
        public bool Result { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
