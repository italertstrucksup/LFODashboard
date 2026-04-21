using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProfileService_LFO.Model.Models
{
    public class UpsertProfileRequest
    {

        [Required]
        public long LoginId { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProfileName { get; set; }

        [Required]
        [Phone]
        [MaxLength(15)]
        public string MobileNo { get; set; }

        [Required]
        [MaxLength(100)]
        public string CompanyType { get; set; }

        [MaxLength(200)]
        public string? CompanyName { get; set; }

        [Required]
        [MaxLength(10)]
        public string Pincode { get; set; }

        [MaxLength(500)]
        public string? CompanyAddress { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string? SubCity { get; set; }

        [Required]
        [MaxLength(100)]
        public string State { get; set; }

        [MaxLength(200)]
        public string? ProfilePhoto { get; set; }

        public bool IsProfilePhotoUploaded { get; set; }

        [MaxLength(200)]
        public string? ProfilePhotoLink { get; set; }

        public DateTime? ProfilePhotoLinkExpiry { get; set; }

        public bool IsKYCDone { get; set; }

        [MaxLength(50)]
        public string? KYCType { get; set; }

        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; set; }
    }
}
