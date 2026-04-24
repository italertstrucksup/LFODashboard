using Common.Core;
using System;
using System.Collections.Generic;
using System.Text;
using VendorService.BL.Interface;
using VendorService.DAL.Interface;
using VendorService.Models;

namespace VendorService.BL.Implementation
{
    public class PanService : IPanService
    {
        private readonly IPanDal _panDal;
        public PanService(IPanDal panDal) { 
            _panDal = panDal;
        }
        public async Task<ApiResponse<PanApiResponse>> VerifyPanAsync(PanVerifyRequest request)
        {
            if (request == null)
                throw new AppException("Request cannot be null");

            if (string.IsNullOrEmpty(request.Pan_number))
                throw new AppException("Pan Number is empty");

            if (request.Date_of_birth == default(DateTime))
                throw new AppException("Date of Birth is not provided");

            // Call DAL
            var result = await _panDal.VerifyPanAsync(request);

            if (result == null)
                throw new AppException("Something went wrong, please try again later", 500);

            return result;
        }
    }
}
