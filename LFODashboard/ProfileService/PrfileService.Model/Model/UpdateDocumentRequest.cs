using System;
using System.Collections.Generic;
using System.Text;

namespace ProfileService_LFO.Model.Model
{
    public class UpdateDocumentRequest
    {
        public Guid? UserId { get; set; }  
        public List<Documents> documents { get; set; }

    }

    public class Documents
    {
        public string DocumentType { get; set; }
        public string DocumentUrl { get; set; }
    }
}
