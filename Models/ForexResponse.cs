using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAPI.Models
{
    public class ForexResponse
    {
        public bool success { get; set; }

        public Payload payload { get; set; }
    }

    public class Payload
    { 
        public Decimal high { get; set; }
        public Decimal last { get; set; }
        public DateTime created_at { get; set; }
        public String book { get; set; }
        public Decimal volume { get; set; }
        public Decimal vwap { get; set; }
        public Decimal low { get; set; }
        public Decimal ask { get; set; }
        public Decimal bid { get; set; }
        public Decimal change_24 { get; set; }
    }
}
