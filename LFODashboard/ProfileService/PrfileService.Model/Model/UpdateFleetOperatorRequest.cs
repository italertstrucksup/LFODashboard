using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProfileService_LFO.Model.Model
{
    public class UpdateFleetOperatorRequest
    {

        [Required]
        public Guid UserId { get; set; }

        
        public string ProfileName { get; set; }


        [MaxLength(200)]
        public string? CompanyName { get; set; }

        
        public string Pincode { get; set; }

        [MaxLength(500)]
        public string? CompanyAddress { get; set; }
        [MaxLength(500)]
        public string? OwnerName { get; set; }
        [Required]

        public string? OpretarType { get; set; }

        
        public string City { get; set; }

        [MaxLength(100)]
        public string? SubCity { get; set; }

       
        public string State { get; set; }

      
        public string UpdatedBy { get; set; }
    }
}
