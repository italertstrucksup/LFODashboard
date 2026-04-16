using System;
using System.Collections.Generic;
using System.Text;
using VendorService.Models;

namespace VendorService.BL.Interface
{
    public interface IAadharService
    {
        Task<AadharResponse> SendAadharOTPAsync(AadhaarRequest request);
    }
}
