using System;
using System.Collections.Generic;
using System.Text;

namespace ProfileService_LFO.Model.Model
{
    public class CompleteKYCResponse
    {
        public Step1BasicDetails Step1 { get; set; }
        public Step2CompanyDetails Step2 { get; set; }
        public object Step3_TruckDetails { get; set; }
        public object Step4_PreferredLanes { get; set; }
        public object Step5_KYCDetails { get; set; }
    }

    public class Step1BasicDetails
    {
        public string ProfileName { get; set; }
        public string MobileNo { get; set; }
        public string OperatorType { get; set; }
        public string ProfilePhoto { get; set; }
    }

    public class Step2CompanyDetails
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Pincode { get; set; }
        public string City { get; set; }
        public string SubCity { get; set; }
        public string State { get; set; }
    }
}


