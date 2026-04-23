using Common.Core;
using System;
using System.Collections.Generic;
using System.Text;
using VendorService.BL.Interface;
using VendorService.DAL.Interface;
using VendorService.Models;

namespace VendorService.BL.Implementation
{
    public class RCDetailService : IRCDetailsService
    {
        public readonly IRCDetailsDAL _rCDetailsDAL;

        public RCDetailService(IRCDetailsDAL rCDetailsDAL)
        {
            _rCDetailsDAL = rCDetailsDAL;
        }

        public async Task<ApiResponse<RcDetailsResponse>> SendRCVerifyAsync(RCDetailsAPIRequest request)
        {
            if (request == null)
                throw new AppException("Request cannot be null");

            if (string.IsNullOrEmpty(request.rc_number))
                throw new AppException("Aadhar Number is empty");

            // Call DAL
            var result = await _rCDetailsDAL.SendRCVerifyAsync(request);

            if (result == null)
                throw new AppException("Something went wrong, please try again later", 500);

            return result;
        }
    }
}
