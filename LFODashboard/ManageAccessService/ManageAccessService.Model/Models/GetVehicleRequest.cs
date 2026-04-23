using System;
using System.Collections.Generic;
using System.Text;

namespace ManageAccessService.Model.Models
{
    public class GetVehicleRequest
    {
        public string? userId { get; set; }
        public string? subUserId { get; set; }
        public List<string>? Vehicles { get; set; } 
        public int? accessType { get; set; } 
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
    }
}
