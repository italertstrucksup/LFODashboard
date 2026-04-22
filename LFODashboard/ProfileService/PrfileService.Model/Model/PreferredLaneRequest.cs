using System;
using System.Collections.Generic;
using System.Text;

namespace ProfileService_LFO.Model.Model
{
    public class PreferredLaneRequest
    {
        public long OperatorId { get; set; }

        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
       
    }
}
