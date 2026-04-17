using System;
using System.Collections.Generic;
using System.Text;

namespace AuthServices_LFO.Model.Models
{
    public class OtpVendorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
