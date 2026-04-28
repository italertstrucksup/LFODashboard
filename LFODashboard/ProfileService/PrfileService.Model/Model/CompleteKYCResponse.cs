using System;
using System.Collections.Generic;
using System.Text;

namespace ProfileService_LFO.Model.Model
{
    public class CompleteKYCResponse
    {
        public OperatorDetails OperatorDetails { get; set; }
        public List<RegistrationDocument> Documents { get; set; }
        public List<TruckDetail> TruckDetails { get; set; }
        public List<PreferredLane> PreferredLanes { get; set; }
        public KYCDetails KYCDetails { get; set; }
    }

    public class OperatorDetails
    {
        public Guid Id { get; set; }
        public string OwnerName { get; set; }
        public string OperatorType { get; set; }
        public string ProfileImage { get; set; }
        public string KYCStatus { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string PinCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string SubCity { get; set; }
        public string CIN { get; set; }
        public string GSTNumber { get; set; }
        public string PANNumber { get; set; }
    }

    public class RegistrationDocument
    {
        public Guid Id { get; set; }
        public string DocumentType { get; set; }
        public string DocumentUrl { get; set; }
        public string DocumentNumber { get; set; }
        public bool IsVerified { get; set; }
    }

    public class TruckDetail
    {
        public long Id { get; set; }
        public string VehicleNo { get; set; }
        public string OwnershipType { get; set; }
        public int BodyTypeId { get; set; }
        public int TyreId { get; set; }
        public int CapacityId { get; set; }
        public int SizeId { get; set; }
    }

    public class PreferredLane
    {
        public long Id { get; set; }
        public string MobileNo { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string FromState { get; set; }
        public string ToState { get; set; }
    }

    public class KYCDetails
    {
        public long Id { get; set; }
        public string KYCType { get; set; }
        public string KYCNumber { get; set; }
        public string KYCProfileImage { get; set; }
        public string KYCDocFront { get; set; }
        public string KYCDocBack { get; set; }
        public bool IsOTPVerified { get; set; }
    }
}
