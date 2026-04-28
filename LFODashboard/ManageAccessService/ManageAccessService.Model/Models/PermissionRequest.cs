using System;
using System.Collections.Generic;
using System.Text;

namespace ManageAccessService.Model.Models
{
    public class PermissionRequest
    {
        public string? UserId { get; set; }
        public int? AccessType { get; set; }
        public int? ModuleId { get; set; }

    }
}
