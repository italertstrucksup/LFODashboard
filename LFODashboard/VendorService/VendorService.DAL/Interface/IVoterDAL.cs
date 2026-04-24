using Common.Core;
using System;
using System.Collections.Generic;
using System.Text;
using VendorService.Models;

namespace VendorService.DAL.Interface
{
    public interface IVoterDAL
    {
        Task<ApiResponse<VIDVerificationResponse>> VerifyVoterAsync(VIDVerificationRequest request);
    }
}
