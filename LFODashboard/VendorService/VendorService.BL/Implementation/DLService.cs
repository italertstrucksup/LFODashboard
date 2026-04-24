using Common.Core;
using System;
using System.Collections.Generic;
using System.Text;
using VendorService.BL.Interface;
using VendorService.DAL.Interface;
using VendorService.Models;

namespace VendorService.BL.Implementation
{

    public class DLService : IDLService
    {
        private readonly IDLDAL _iddal;
        public DLService(IDLDAL iddal) 
        {
            _iddal = iddal;
        }

        public async Task<ApiResponse<DrivingLicenceResponse>> VerifyDrivingLicence(DrivingLicenceRequest drivingLicenceRequest)
        {
            if (drivingLicenceRequest == null)
                throw new AppException("Request cannot be null");

            if (string.IsNullOrEmpty(drivingLicenceRequest.DrivingLicense))
                throw new AppException("Driving License Number is empty");

            if (drivingLicenceRequest.DateOfBirth == default(DateTime))
                throw new AppException("Date of Birth is not provided");

            // Call DAL
            var result = await _iddal.VerifyDrivingLicence(drivingLicenceRequest);

            if (result == null)
                throw new AppException("Something went wrong, please try again later", 500);

            return result;
        }
    }
}
