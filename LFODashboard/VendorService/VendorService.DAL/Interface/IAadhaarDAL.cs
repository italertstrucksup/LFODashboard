using System;
using System.Collections.Generic;
using System.Text;
using VendorService.Models;

namespace VendorService.DAL.Interface
{
    public interface IAadhaarDAL
    {
        Task<AadharResponse> SendAadharOTPAsync(AadhaarRequest request);
    }
}
