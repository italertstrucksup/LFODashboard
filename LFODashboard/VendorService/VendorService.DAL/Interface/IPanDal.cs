using Common.Core;
using System;
using System.Collections.Generic;
using System.Text;
using VendorService.Models;

namespace VendorService.DAL.Interface
{
    public interface IPanDal
    {
        Task<ApiResponse<PanApiResponse>> VerifyPanAsync(PanVerifyRequest request);
    }
}
