using System;
using System.Collections.Generic;
using System.Text;

namespace ProfileService_LFO.Model.Models
{
    public class KYCRequest
    {
        public long ProfileId { get; set; }
        public string KYCType { get; set; }
    }
}
