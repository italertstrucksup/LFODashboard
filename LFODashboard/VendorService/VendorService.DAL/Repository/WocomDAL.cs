using Microsoft.Extensions.Configuration;
using VendorService.DAL.Interface;
using VendorService.Models;

namespace VendorService.DAL.Repository
{
    public class WocomDAL : IWocomDAL
    {
        private readonly IConfiguration _configurationBuilder;

        public WocomDAL(IConfiguration configurationBuilder)
        {
            _configurationBuilder = configurationBuilder;
        }

        public async Task<WocomResponse> LoginOTP(string mobile, string otp)
        {
            string message = $"Verification code is {otp} for TrucksUp Login against for Reference No. uc391mZzsTs";
            return await ExecuteSendSMS(mobile, message);
        }

        public async Task<WocomResponse> ResetOTP(string mobile, string otp)
        {
            string message = $"Verification code is {otp} for TrucksUp Login against for Reference No. uc391mZzsTs";
            return await ExecuteSendSMS(mobile, message);
        }

        public async Task<WocomResponse> SignUpOTP(string mobile, string otp)
        {
            string message = $"Verification code is {otp} for TrucksUp Login against for Reference No. uc391mZzsTs";
            return await ExecuteSendSMS(mobile, message);
        }

        private async Task<WocomResponse> ExecuteSendSMS(string mobile, string message)
        {
            WocomResponse response = new WocomResponse();
            try
            {
                var client = new HttpClient();
                string baseurl = _configurationBuilder.GetValue<string>("WoComBaseUrl");
                string tempid = _configurationBuilder.GetValue<string>("templateid");
                string entityId = _configurationBuilder.GetValue<string>("entityId");
                string apikey = _configurationBuilder.GetValue<string>("apikey");
                string smsUsername = _configurationBuilder.GetValue<string>("smsUsername");
                string smsSignature = _configurationBuilder.GetValue<string>("smsSignature");

                string  requestUrl = @$"{baseurl}username={smsUsername}&dest={mobile}&apikey={apikey}&signature={smsSignature}&msgtype=PM&msgtxt={message}&entityid={entityId}&templateid={tempid}";
                var WoComrequest = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                
                var WoComresponse = await client.SendAsync(WoComrequest);
                
                if (WoComresponse.IsSuccessStatusCode)
                {
                    response.Message = "Otp Sent successfully";
                    response.StatusCode = 200;
                }
                else
                {
                    response.Message = "Failed to send OTP";
                    response.StatusCode = (int)WoComresponse.StatusCode;
                }
            }
            catch (Exception)
            {
                response.Message = "Failed to send OTP due to internal error";
                response.StatusCode = 500;
            }

            return response;
        }
    }
}
