using System;
using System.Collections.Generic;
using System.Text;

namespace ProfileService_LFO.Model.Models
{
    public class UpsertDocumentRequest
    {
        public long ProfileId { get; set; }

        public string COI_File { get; set; }
        public string PAN_File { get; set; }
        public string MAA_File { get; set; }
        public string GST_File { get; set; }
        public string RC_File { get; set; }
        public string Partnership_File { get; set; }

        public string VerifiedBy { get; set; }
    }
}
