using Common.Core;
using System;
using System.Collections.Generic;
using System.Text;
using VendorService.Models;

namespace VendorService.BL.Interface
{
    public interface IPanService
    {
        Task<ApiResponse<PanApiResponse>> VerifyPanAsync(PanVerifyRequest request);
    }
}
