using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using WcfAirportManagerLib;

namespace WcfAirportManagerHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://localhost:8000/AirportManager/");
            ServiceHost selfHost = new ServiceHost(typeof(AirportService), baseAddress);
            try
            {
                selfHost.AddServiceEndpoint(typeof(IAirportService), new WSHttpBinding(), "AiportService");
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true
                };
                selfHost.Description.Behaviors.Add(smb);

                selfHost.Open();

                Console.WriteLine("The service is ready");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.WriteLine();
                Console.ReadLine();

                selfHost.Close();
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine("An exception occurred: {0}", ex.Message);
                selfHost.Abort();
            }
        }
    }
}
