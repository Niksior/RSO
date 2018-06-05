using AirportResourcesService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AirportResources
{
    public class AirConnectionsReader : IAirConnectionsReader
    {
        public IList<IAirConnection> LoadDatabase(String path)
        {
            IList<IAirConnection> airConnections = new List<IAirConnection>();
            var csvRows = File.ReadAllLines(path, Encoding.Default).ToList();
            foreach (var row in csvRows.Skip(1))
            {
                var columns = row.Split(',');
                AirConnection airData = new AirConnection(columns);
                airConnections.Add(airData);
            }
            return airConnections;
        }

    }
}
