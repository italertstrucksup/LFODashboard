using Common.Core;
using System;
using System.Collections.Generic;
using System.Text;
using VendorService.Models;

namespace VendorService.BL.Interface
{
    public interface IVoterService
    {
        Task<ApiResponse<VIDVerificationResponse>> VerifyVoterAsync(VIDVerificationRequest request);
    }
}
