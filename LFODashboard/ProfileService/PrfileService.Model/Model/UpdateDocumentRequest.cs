using System;
using System.Collections.Generic;
using System.Text;

namespace ProfileService_LFO.Model.Model
{
    public class UpdateDocumentRequest
    {
        public Guid? UserId { get; set; }
        public Guid OperatorId { get; set; }

        public string DocumentType { get; set; } 
        public string DocumentUrl { get; set; }  

    }
}
