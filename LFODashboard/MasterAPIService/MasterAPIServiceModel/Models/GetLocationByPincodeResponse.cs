using System;
using System.Collections.Generic;
using System.Text;

namespace MasterAPIServiceModel.Models
{
    public class GetLocationByPincodeResponse
    {
        public string? CircleName { get; set; }
        public string? RegionName { get; set; }
        public string? DivisionName { get; set; }
        public string? SubCity { get; set; }
        public string? PinCode { get; set; }
        public string? OfficeType { get; set; }
        public string? Delivery { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool ActiveFlag { get; set; }
        public DateTime? CreatedAt { get; set; }


    }
}
