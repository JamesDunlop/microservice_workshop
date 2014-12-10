using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RentalOffer.Core;

namespace RentalOffer.Monitor
{

    public class Location
    {

        private readonly string busName;

        public static void Main(string[] args)
        {
            string host = args[0];
            string busName = args[1];

            new Connection(host, busName).WithOpen(new Location(busName).MonitorNeeds);
        }

        public Location(string busName)
        {
            this.busName = busName;
        }

        private void MonitorNeeds(Connection connection)
        {
            var sub = connection.Subscribe();
            Console.WriteLine(" [*] Location offer service waiting for needs on the {0} bus... To exit press CTRL+C", busName);

            while (true)
            {
                var e = sub.Next();
                var json = Encoding.UTF8.GetString(e.Body);

                var message = JsonConvert.DeserializeObject<JObject>(json);

                var existingUser = message["user"];
                var existingSolutions = message["solutions"];
                bool hasNoSolutions = existingSolutions != null && !existingSolutions.Any();

                if (message != null && hasNoSolutions && existingUser != null && existingUser["location"] != null)
                {
                    var solutions = new JArray();

                    var solution = new JObject(
                        new JProperty("offer", "location discount"),
                        new JProperty("discount", "15%")
                        );

                    solutions.Add(solution);

                    message["solutions"] = solutions;

                    connection.Publish(JsonConvert.SerializeObject(message));
                }
            }

        }


    }

}
