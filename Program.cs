using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace TurnsTileD_SIM
{
    class Program
    {
        static DeviceClient deviceClient;
        static string deviceConnectionString = "HostName=OHIoTTeam101.azure-devices.net;DeviceId=TurnsTileD;SharedAccessKey=BseSyyygZkau1Glnb6CfvMWfOuQcaUerYJXTknSbobg=";

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated TurnsTile\n");
            deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Http1);
            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }

        private static async void SendDeviceToCloudMessagesAsync()
        {
            Guid ticketID;
            DateTime entryTime;

            while (true)
            {
                var telemetryDataPoint = new
                {
                   ticketID = Guid.NewGuid(),
                   entryTime = DateTime.Now
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                await Task.Delay(1000);
            }
        }
    }
}
