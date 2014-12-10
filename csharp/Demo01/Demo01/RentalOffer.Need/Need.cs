using System;

using RentalOffer.Core;

namespace RentalOffer.Need {

    public class Need {

        private readonly string busName;

        public static void Main(string[] args) {
            string host = args[0];
            string busName = args[1];

            var conn = new Connection(host, busName);

            while (true)
            {
                Console.ReadKey();
                conn.WithOpen(new Need(busName).PublishNeed);
            }
        }

        public Need(string busName) {
            this.busName = busName;
        }

        private void PublishNeed(Connection connection) {
        }

    }

}
