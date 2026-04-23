using System;
using System.Collections.Generic;
using System.Text;

namespace ManageAccessService.Model.Models
{
    public class AssignVehicleRequest
    {
        public string UserId { get; set; }
        public int CreatedBy { get; set; }
        public List<string> Vehicles { get; set; }
    }
}
