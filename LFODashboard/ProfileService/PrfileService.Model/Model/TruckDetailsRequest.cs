using System;
using System.Collections.Generic;
using System.Text;

namespace ProfileService_LFO.Model.Model
{
    public class TruckDetailsRequest
    {
        public Guid? UserId { get; set; }

        public string VehicleNo { get; set; }
        public string OwnershipType { get; set; }
        public int BodyTypeId { get; set; }
        public int CapacityId { get; set; }
        public int SizeId { get; set; }
        public int TyreId { get; set; }
    }
}
