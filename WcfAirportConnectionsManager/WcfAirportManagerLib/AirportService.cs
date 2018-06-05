using AirportResources;
using AirportResourcesService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.Text.RegularExpressions;

namespace WcfAirportManagerLib
{
    public class AirportService : IAirportService
    {
        private IAirConnectionsDatabase airConnectionsDatabase;

        public AirportService()
        {
            string csvPath = ConfigurationManager.AppSettings["CsvDatabasePath"];
            airConnectionsDatabase = new AirConnectionsDatabase(csvPath);
        }

        public IList<AirConnection> GetAirConnections(string portA, string portB)
        {
            CheckValidityOfAirports(portA, portB);
            IList <AirConnection> list = new List<AirConnection>();
            foreach (AirConnection conn in airConnectionsDatabase.GetAirConnections(portA, portB))
            {
                list.Add(conn);
            }
            foreach(AirConnection conn in airConnectionsDatabase.GetIndirectAirConnections(portA, portB))
            {
                list.Add(conn);
            }

            if (list.Count == 0)
            {
                throw new FaultException<NoConnectionsFault>(new NoConnectionsFault(), new FaultReason("There is no any connection between those Airports!"));
            }
            
            return list;
        }

        private void CheckValidityOfAirports(string portA, string portB)
        {
            if (!Regex.IsMatch(portA, @"^[a-zA-Z]+$") || !Regex.IsMatch(portB, @"^[a-zA-Z]+$"))
                throw new FaultException<InvalidInputFault>(new InvalidInputFault(), "Provided airport name(s) is/are invalid.");
            if (String.Equals(portA, portB, StringComparison.OrdinalIgnoreCase))
                throw new FaultException<InvalidInputFault>(new InvalidInputFault(), "Provided airports are identical!");
            string errorMsg = "";
            if (!airConnectionsDatabase.ContainsAirport(portA))
                errorMsg += portA + " ";
            if (!airConnectionsDatabase.ContainsAirport(portB))
                errorMsg += portB + " ";
            if (errorMsg.Length > 0)
            {
                throw new FaultException<InvalidAirportFault>(new InvalidAirportFault(), new FaultReason(String.Format("There is no such airport(s): {0}", errorMsg)));
            }
        }

        public IList<AirConnection> GetAirConnections(string portA, string portB, DateTime departure, DateTime arrival)
        {
            CheckValidityOfAirports(portA, portB);
            if (departure >= arrival) throw new FaultException<InvalidInputFault>(new InvalidInputFault(), "Provided arrival time is not later than departure!");
            IList<AirConnection> list = new List<AirConnection>();
            foreach (AirConnection conn in airConnectionsDatabase.GetAirConnections(portA, portB, departure, arrival))
            {
                list.Add(conn);
            }
            foreach(AirConnection conn in airConnectionsDatabase.GetIndirectAirConnections(portA, portB, departure, arrival))
            {
                list.Add(conn);
            }

            if (list.Count == 0)
                throw new FaultException<NoConnectionsFault>(new NoConnectionsFault(), new FaultReason("There is no any connection between those airports in that time range!"));
            return list;
        }

    }
}
