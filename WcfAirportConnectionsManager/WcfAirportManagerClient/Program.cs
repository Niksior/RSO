using System;


namespace WcfAirportManagerClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (AirportManager manager = new AirportManager())
            {
                manager.HandleClientInputsInLoop();
            }
            Console.ReadKey();
        }
    }
}
