using System;
using System.Collections.Generic;
using System.Text;

namespace MasterAPIServiceModel.Models
{
    public class CityMasterResponse
    {
        public int SrNo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Pincode1 { get; set; }
        public int Pincode2 { get; set; }
    }
}
