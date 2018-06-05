using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WcfAirportManagerClient
{
    class AirportManager : IDisposable
    {
        private static int QuitInputLenght => 1;
        private static int AmountParametersForAllConnections => 2;
        private static int AmountParametersForConnectionInTimeRange => 4;
        private ServiceClient serviceClient;

        public AirportManager() => serviceClient = new ServiceClient();

        public void HandleClientInputsInLoop()
        {
            bool nextIteration = true;
            do {
                try {
                    DisplayPrompt();
                    nextIteration = GetInput();
                } catch (InvalidInputException ex) {
                    Console.Error.WriteLine(String.Format("ERROR: {0}. Please, next time provide valid arguemnts.", ex.Message));
                }
            } while (nextIteration);
            Console.Out.WriteLine("Bye");
        }

        private bool GetInput()
        {
            string sourcePort = String.Empty;
            string destPort = String.Empty;

            if (!GetInputArgument("Enter the source airport: ", ref sourcePort) || !GetInputArgument("Enter the destination airport: ", ref destPort)) 
                return false;

            if (!Regex.IsMatch(sourcePort, @"^[a-zA-Z]+$") || !Regex.IsMatch(destPort, @"^[a-zA-Z]+$"))
                throw new InvalidInputException("Provided airport name(s) is/are invalid.");

            if (String.Equals(sourcePort, destPort, StringComparison.OrdinalIgnoreCase))
                throw new InvalidInputException("Provided airports are identical!");

            IList<string> input = new List<string>
            {
                sourcePort,
                destPort
            };

            Console.WriteLine("Do you want to enter time ranges? Pres Y, if yes. Q to quit. Otherwise all available connections will be printed.");
            var inputTimeChoiseKey = Console.ReadKey();
            if (inputTimeChoiseKey.Key.Equals(ConsoleKey.Q)) return false;
            if (inputTimeChoiseKey.Key.Equals(ConsoleKey.Y))
            {
                string fromTime = String.Empty;
                string toTime = String.Empty;
                if (!GetInputArgument("Enter the departure time: ", ref fromTime) ||
                    !GetInputArgument("Enter the arrival time: ", ref toTime))
                {
                    return false;
                }
                input.Add(fromTime);
                input.Add(toTime);
            }

            HandleInput(input);
            return true;
        }

        private bool GetInputArgument(string prompt, ref string airport)
        {
            Console.WriteLine(prompt);
            airport = Console.ReadLine();
            return !WantToExit(airport);
        }

        private bool WantToExit (string input) => input.Length == QuitInputLenght && Char.ToUpper(input[0]).Equals('Q');

        private void HandleInput(IList<string> input)
        {
            IList<AirportResources.AirConnection> connections = new List<AirportResources.AirConnection>();
            if (input.Count == AmountParametersForAllConnections)
            {
                connections = serviceClient.GetAirConnections(input[0], input[1], null, null);
            }
            else if (input.Count == AmountParametersForConnectionInTimeRange)
            {
                string portA = input[0];
                string portB = input[1];
                if (!DateTime.TryParse(input[2], out DateTime departureTime) || !DateTime.TryParse(input[3], out DateTime arrivalTime))
                    throw new InvalidInputException("The provided time cannot be parsed.");
                if (departureTime >= arrivalTime) throw new InvalidInputException("Provided arrival time should be later than departure");
                connections = serviceClient.GetAirConnections(portA, portB, departureTime, arrivalTime);
            }
            else
                throw new InvalidInputException();
            if (connections != null) PrintConnections(connections);
        }

        public void PrintConnections(IList<AirportResources.AirConnection> connections)
        {
            foreach (var conn in connections)
            {
                Console.Out.WriteLine(String.Format("From: {0}\t To: {1}\t Departure: {2}\t Arrival: {3}", conn.AirportA, conn.AirportB, conn.DepartureTime, conn.ArrivalTime));
                foreach (var conn2 in conn.Connections)
                {
                    Console.Out.WriteLine(String.Format("From: {0}\t To: {1}\t Departure: {2}\t Arrival: {3}", conn2.AirportA, conn2.AirportB, conn2.DepartureTime, conn2.ArrivalTime));
                }
                Console.Out.WriteLine();
            }
        }

        private void DisplayPrompt()
        {
            Console.WriteLine("If you want to quit, enter Q.");
        }

        public void Dispose()
        {
            serviceClient.Dispose();
        }
    }

}
