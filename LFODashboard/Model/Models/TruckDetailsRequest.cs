using System;
using System.Collections.Generic;
using System.Text;

namespace ProfileService_LFO.Model.Models
{
    public class TruckDetailsRequest
    {
        public long ProfileId { get; set; }

        public string TruckNumber { get; set; }
        public string OwnershipType { get; set; }
        public string BodyType { get; set; }
        public int TyreCount { get; set; }
        public decimal Capacity { get; set; }
        public string VehicleSize { get; set; }
    }
}
