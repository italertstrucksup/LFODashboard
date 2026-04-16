using System;
using System.Collections.Generic;
using System.Text;

namespace VendorService.Models
{
    public class RequestModel
    {

    }

    public class OtpRequest
    {
        public string mobile { get; set; }
        public string otp { get; set; }
        public string otptype { get; set; }

    }
}
