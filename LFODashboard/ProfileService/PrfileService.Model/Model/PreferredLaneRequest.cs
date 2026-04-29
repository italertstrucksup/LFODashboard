using System;
using System.Collections.Generic;
using System.Text;

namespace ProfileService_LFO.Model.Model
{
    public class PreferredLaneRequest
    {
        public Guid? UserId { get; set; }

        public List<PreferredLaneModel> Lanes { get; set; }
    }

    public class PreferredLaneModel
    {
        public string FromLocation { get; set; }

        public string ToLocation { get; set; }

        public string FromState { get; set; }

        public string ToState { get; set; }
    }
}