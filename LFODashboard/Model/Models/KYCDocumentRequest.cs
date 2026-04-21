using System;
using System.Collections.Generic;
using System.Text;

namespace ProfileService_LFO.Model.Models
{
    public class KYCDocumentRequest
    {
        public long KYCId { get; set; }

        public string ProfilePhoto { get; set; }

        public string AadhaarNumber { get; set; }
        public string AadhaarFront { get; set; }
        public string AadhaarBack { get; set; }
        public string AadhaarSelfie { get; set; }
        public string AadhaarAddress { get; set; }

        public string PANNumber { get; set; }
        public string PANName { get; set; }
        public DateTime? PANDOB { get; set; }
        public string PANFile { get; set; }
        public string PANAddress { get; set; }

        public string VoterIdNumber { get; set; }
        public string VoterIdFront { get; set; }

        public string DLNumber { get; set; }
        public DateTime? DLDOB { get; set; }
        public string DLFront { get; set; }
        public string DLBack { get; set; }

        public string SelfieKey { get; set; }
        public bool IsSelfieUploaded { get; set; }
        public string SelfieLink { get; set; }
        public string VerificationCode { get; set; }
    }
}
