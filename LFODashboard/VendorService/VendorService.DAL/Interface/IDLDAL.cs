using Common.Core;
using System;
using System.Collections.Generic;
using System.Text;
using VendorService.Models;

namespace VendorService.DAL.Interface
{
    public interface IDLDAL
    {
        Task<ApiResponse<DrivingLicenceResponse>> VerifyDrivingLicence(DrivingLicenceRequest drivingLicenceRequest);
    }
}
