using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing.Impl;
using RentalOffer.Core;
using Connection = RentalOffer.Core.Connection;

namespace RentalOffer.Manager {

    public class Manager
    {

        private readonly string busName;

        public static void Main(string[] args)
        {
            string host = args[0];
            string busName = args[1];

            new Connection(host, busName).WithOpen(new Manager(busName).MonitorNeeds);
        }

        public Manager(string busName)
        {
            this.busName = busName;
        }

        private void MonitorNeeds(Connection connection)
        {
            var sub = connection.Subscribe();
            Console.WriteLine(" [*] Rental offer manager service waiting for needs with solutions on the {0} bus... To exit press CTRL+C", busName);

            while (true)
            {
                var e = sub.Next();
                var message = Encoding.UTF8.GetString(e.Body);

                Thread.Sleep(2000);


            }
        }

    }

}
