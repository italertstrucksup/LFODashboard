using System;
using System.Collections.Generic;
using System.Text;

namespace ProfileService_LFO.Model.Model
{
    public class ProfileResponse
    {
        public string Status { get; set; }
        public long ProfileId { get; set; }
        public long UserId { get; set; }
        public string MobileNo { get; set; }
        public string ProfileName { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string IsKYCDone { get; set; }
        public string Message { get; set; }
    }
}
