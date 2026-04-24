using System;
using System.Collections.Generic;
using System.Text;

namespace VendorService.Models
{
    public class PanVerifyRequest
    {
        public string Pan_number { get; set; }
        public string Name { get; set; }
        public DateTime? Date_of_birth { get; set; }

    }

    public class PanApiResponse
    {
        public int statuscode { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public int reference_id { get; set; }
        public PanVerifiedResponse data { get; set; }
    }

    public class PanVerifiedResponse
    {

        public string idNumber { get; set; }
        public string idStatus { get; set; }
        public string category { get; set; }
        public string panStatus { get; set; }
        //public PanAddress? address { get; set; } = null;
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string fullName { get; set; }
        public string idHolderTitle { get; set; }
        public string fatherName { get; set; }
        public string idLastUpdated { get; set; }
        public string aadhaarSeedingStatus { get; set; }
        public string maskedAadhaar { get; set; }
    }

    public class PanAddress
    {
        public string line_1 { get; set; }
        public string line_2 { get; set; }
        public string street_name { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string full { get; set; }
    }

}
