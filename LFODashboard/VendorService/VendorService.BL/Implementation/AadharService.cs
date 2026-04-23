using Common.Core;
using Microsoft.Extensions.Configuration;
using VendorService.BL.Interface;
using VendorService.DAL.Interface;
using VendorService.Models;

public class AadharService : IAadharService
{
    private readonly IAadhaarDAL _aadhardal;

    public AadharService(IAadhaarDAL aadhardal)
    {
        _aadhardal = aadhardal;
    }


    public async Task<AadharResponse> SendAadharOTPAsync(AadhaarRequest request)
    {
        // Validation
        if (request == null)
            throw new AppException("Request cannot be null");

        if (string.IsNullOrEmpty(request.aadhaar_number))
            throw new AppException("Aadhar Number is empty");

        // Call DAL
        var result = await _aadhardal.SendAadharOTPAsync(request);

        if (result == null)
            throw new AppException("Something went wrong, please try again later", 500);

        return result;
    }

    public async Task<ApiResponse<AadharVerifyResponse>> VerifyAadharAsync(AadhaarVerifyRequest request)
    {
        // Validation
        if (request == null)
            throw new AppException("Request cannot be null");

        // Call DAL
        var result = await _aadhardal.VerifyAadharAsync(request);

        if (result == null)
            throw new AppException("Something went wrong, please try again later", 500);

        return result;
    }
}