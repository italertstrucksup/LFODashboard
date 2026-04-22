using System;
using System.Collections.Generic;
using System.Text;

namespace ProfileService_LFO.Model.Model
{
    public class KYCRequest
    {
        public long ProfileId { get; set; }
        public string KYCType { get; set; }
        public string KYCNumber { get; set; }
        public string KYCDocFront { get; set; }
        public string KYCDocBack { get; set; }
    }
}
