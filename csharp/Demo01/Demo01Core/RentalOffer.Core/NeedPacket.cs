using System;
using System.Collections.Generic;

using fastJSON;

namespace RentalOffer.Core {

    public class NeedPacket {

        public List<object> Solutions { get; set; }

        public NeedPacket()
        {
            Solutions = new List<object>();
        }

        public string ToJson() {
            return JSON.ToJSON(this);
        }

        public void ProposeSolution(Object solution) {
            Solutions.Add(solution);
        }

    }

}

