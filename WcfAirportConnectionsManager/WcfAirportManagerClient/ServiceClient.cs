using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace WcfAirportManagerClient
{
    class ServiceClient : IDisposable
    {
        private AirportServiceClient client;

        public ServiceClient() => client = new AirportServiceClient();

        public void Dispose()
        {
            client.Close();
        }

        public IList<AirportResources.AirConnection> GetAirConnections(string portA, string portB, DateTime? from, DateTime? to)
        {
            try
            {
                if (from.HasValue && to.HasValue)
                    return client.GetAirConnections(portA, portB, from.Value, to.Value);
                else
                    return client.GetAllAirConnections(portA, portB);
            }
            catch (TimeoutException timeProblem)
            {
                Console.Error.WriteLine(String.Format("The service operation timed out. {0}", timeProblem.Message));
                client.Abort();
                Console.ReadKey();
                Environment.Exit(1);
            }
            catch (FaultException<WcfAirportManagerLib.NoConnectionsFault> ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            catch (FaultException<WcfAirportManagerLib.InvalidInputFault> ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            catch (FaultException<WcfAirportManagerLib.InvalidAirportFault> ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            catch (CommunicationException commProblem)
            {
                Console.Error.WriteLine(String.Format("There was a communication problem. {0}", commProblem.Message));
                client.Abort();
                Console.ReadKey();
                Environment.Exit(1);
            }
            return null;
        }
    }
}
