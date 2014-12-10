using System;
using System.Text;
using System.Threading;
using fastJSON;
using RentalOffer.Core;

namespace RentalOffer.Monitor
{

    public class Brand
    {

        private readonly string busName;

        public static void Main(string[] args)
        {
            string host = args[0];
            string busName = args[1];

            new Connection(host, busName).WithOpen(new Brand(busName).MonitorNeeds);
        }

        public Brand(string busName)
        {
            this.busName = busName;
        }

        private void MonitorNeeds(Connection connection)
        {
            var sub = connection.Subscribe();
            Console.WriteLine(" [*] Brand offer service waiting for needs on the {0} bus... To exit press CTRL+C", busName);

            while (true)
            {
                var e = sub.Next();
                var message = Encoding.UTF8.GetString(e.Body);

                Thread.Sleep(2000);

                var need = JSON.ToObject(message) as NeedPacket;

                if (need != null && need.Solutions.Count == 0)
                {
                    Console.WriteLine(" [x] Received: {0}", message);
                    need.ProposeSolution("brand offer of 10% discount");
                    PublishBrandOffer(connection, need);
                }
            }
        }

        private void PublishBrandOffer(Connection connection, NeedPacket need) {
            string message = need.ToJson();
            connection.Publish(message);
            Console.WriteLine(" [x] Published {0} on the {1} bus", message, busName);
        }
    }

}
