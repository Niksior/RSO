using AirportResources;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace WcfAirportManagerLib
{
    [ServiceContract]
    public interface IAirportService
    {
        [OperationContract(Name = "GetAllAirConnections")]
        [FaultContract(typeof(InvalidInputFault))]
        [FaultContract(typeof(NoConnectionsFault))]
        [FaultContract(typeof(InvalidAirportFault))]
        IList<AirConnection> GetAirConnections(string portA, string portB);

        [OperationContract(Name = "GetAirConnections")]
        [FaultContract(typeof(InvalidInputFault))]
        [FaultContract(typeof(NoConnectionsFault))]
        [FaultContract(typeof(InvalidAirportFault))]
        IList<AirConnection> GetAirConnections(string portA, string portB, DateTime departure, DateTime arrival);
    }
}