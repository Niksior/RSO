using AirportResourcesService;
using System;
using System.Collections.Generic;

namespace AirportResources
{
    public class AirConnectionsDatabase : IAirConnectionsDatabase
    {
        public IList<IAirConnection> AirConnections { get; private set; }
        private ISet<string> Airports { get;  set; }
        public string CsvPath { get; set; }

        private IAirConnectionsReader connectionsReader;

        public AirConnectionsDatabase(string csvPath)
        {
            CsvPath = csvPath;
            Airports = new HashSet<string>();
            connectionsReader = new AirConnectionsReader();
            LoadAirConnections();
        }

        public void LoadAirConnections()
        {
            AirConnections = connectionsReader.LoadDatabase(CsvPath);
            foreach(var conn in AirConnections)
            {
                Airports.Add(conn.AirportA);
                Airports.Add(conn.AirportB);
            }
        }

        public bool ContainsAirport(string name)
        {
            return Airports.Contains(name);
        }

        public IList<IAirConnection> GetAirConnections (string portA, string portB)
        {
            IList<IAirConnection> connections = new List<IAirConnection>();

            foreach (var conn in AirConnections)
            {
                if (conn.AirportA.Equals(portA) && conn.AirportB.Equals(portB))
                {
                    connections.Add(conn);
                }
            }

            return connections;
        }

        public IList<IAirConnection> GetIndirectAirConnections(string portA, string portB)
        {
            IList<IAirConnection> list1 = new List<IAirConnection>();
            IList<IAirConnection> list2 = new List<IAirConnection>();
            foreach (var conn in AirConnections)
            {
                if (conn.AirportA.Equals(portA))
                    list1.Add(conn);
                if (conn.AirportB.Equals(portB))
                    list2.Add(conn);
            }
            IList<IAirConnection> list3 = new List<IAirConnection>();
            foreach (AirConnection conn in list1)
            {
                foreach (AirConnection conn2 in list2)
                {
                    if (conn.AirportB.Equals(conn2.AirportA) && conn2.DepartureTime >= conn.ArrivalTime)
                    {
                        AirConnection result = (AirConnection) conn.Clone();
                        result.Connections.Add(conn2);
                        list3.Add(result);
                    }
                }
            }
            return list3;
        }

        public IList<IAirConnection> GetIndirectAirConnections(string portA, string portB, DateTime departureTime, DateTime arrivalTime)
        {
            IList<IAirConnection> allConnections = GetIndirectAirConnections(portA, portB);
            IList<IAirConnection> connections = new List<IAirConnection>();
            foreach (AirConnection conn in allConnections)
            {
                foreach (AirConnection conn2 in conn.Connections)
                {
                    if (departureTime <= conn.DepartureTime && departureTime < conn2.ArrivalTime)
                        if (arrivalTime > conn.DepartureTime && arrivalTime >= conn2.ArrivalTime)
                        {
                            connections.Add(conn);
                            break;
                        }
                    
                }
            }
            return connections;
        }

        public IList<IAirConnection> GetAirConnections (string portA, string portB, DateTime departureTime, DateTime arrivalTime)
        {
            IList<IAirConnection> allConnections = GetAirConnections(portA, portB);
            IList<IAirConnection> connections = new List<IAirConnection>();
            foreach (var conn in allConnections)
            {
                if (departureTime <= conn.DepartureTime && departureTime < conn.ArrivalTime)
                    if (arrivalTime > conn.DepartureTime && arrivalTime >= conn.ArrivalTime)
                        connections.Add(conn);
            }
            return connections;
        }
    }
}
