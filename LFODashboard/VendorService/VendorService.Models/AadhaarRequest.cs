using System;
using System.Collections.Generic;
using System.Text;

namespace VendorService.Models
{
    public class AadhaarRequest
    {
        public string? aadhaar_number { get; set; }
    }

    public class AadharResponse
    {
        public int? statuscode { get; set; }
        public bool? status { get; set; }
        public string? message { get; set; }
        public string? hindiMessage { get; set; }
        public string? reference_id { get; set; }
        public string? TransactionId { get; set; }
        public DataResponseData? data { get; set; }
        public string? VerificationThrough { get; set; } = "SPRINTVERIFY";
    }

    public class DataResponseData
    {
        public string? client_id { get; set; }
        public bool otp_sent { get; set; }
        public bool if_number { get; set; }
        public bool valid_aadhaar { get; set; }
        public string? status { get; set; }
    }
}
