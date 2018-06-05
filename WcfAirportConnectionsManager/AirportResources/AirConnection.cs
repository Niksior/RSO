using AirportResourcesService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AirportResources
{
    [DataContract]
    public class AirConnection : IAirConnection, ICloneable
    {
        private int ColumnsAmount => 4;

        [DataMember]
        public String AirportA { get; private set; }
        [DataMember]
        public String AirportB { get; private set; }
        [DataMember]
        public DateTime DepartureTime { get; private set; }
        [DataMember]
        public DateTime ArrivalTime { get; private set; }
        [DataMember]
        public IList<AirConnection> Connections { get; private set; }
        
        private AirConnection() { }

        public AirConnection(string[] columns)
        {
            if (columns.Length < ColumnsAmount)
                return;
            AirportA = columns[0].Trim();
            AirportB = columns[1].Trim();
            DepartureTime = DateTime.Parse(columns[2]);
            ArrivalTime = DateTime.Parse(columns[3]);
            Connections = new List<AirConnection>();
        }
     
        public object Clone()
        {
            AirConnection airConnection = (AirConnection) MemberwiseClone();
            airConnection.Connections = new List<AirConnection>();
            Connections.ToList().ForEach(conn => airConnection.Connections.Add(conn));
            return airConnection;
        }
    }
}
