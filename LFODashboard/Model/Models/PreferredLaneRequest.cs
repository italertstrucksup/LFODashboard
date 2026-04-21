using System;
using System.Collections.Generic;
using System.Text;

namespace ProfileService_LFO.Model.Models
{
    public class PreferredLaneRequest
    {
        public long LoginId { get; set; }

        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public string FromState { get; set; }
        public string ToState { get; set; }
    }
}
