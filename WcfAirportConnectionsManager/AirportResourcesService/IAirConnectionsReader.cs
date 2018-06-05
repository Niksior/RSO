using System.Collections.Generic;

namespace AirportResourcesService
{
    public interface IAirConnectionsReader
    {
        IList<IAirConnection> LoadDatabase(string path);
    }
}
