using Common.Core;
using System;
using System.Collections.Generic;
using System.Text;
using VendorService.BL.Interface;
using VendorService.DAL.Interface;
using VendorService.DAL.Repository;
using VendorService.Models;

namespace VendorService.BL.Implementation
{
    public class VoterService : IVoterService
    {
        private readonly IVoterDAL _voterDAL;
        public VoterService(IVoterDAL voterDAL) 
        {       
            _voterDAL = voterDAL;
        }
        public async Task<ApiResponse<VIDVerificationResponse>> VerifyVoterAsync(VIDVerificationRequest request)
        {
            if (request == null)
                throw new AppException("Request cannot be null");

            if (string.IsNullOrEmpty(request.id_number))
                throw new AppException("Pan Number is empty");

            // Call DAL
            var result = await _voterDAL.VerifyVoterAsync(request);

            if (result == null)
                throw new AppException("Something went wrong, please try again later", 500);

            return result;
        }
    }
}
