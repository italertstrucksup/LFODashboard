using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using MediaService.BL.Model;
using MediaService.BL.IBussinessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text;
using Amazon.S3.Model;
using Common.Core;

namespace MediaServiceAPI.BussinessLayer
{
    public class MediaBusinessLayer : IMediaBusinessLayer
    {
        private readonly S3Settings _s3Settings;
        private readonly IAmazonS3 _s3Client;

        public MediaBusinessLayer(IOptions<S3Settings> s3Settings, IAmazonS3 s3Client)
        {
            _s3Settings = s3Settings.Value ?? throw new ArgumentNullException(nameof(s3Settings));
            _s3Client = s3Client;
        }

        public async Task<ApiResponse<object>> UploadDocumentAsync(IFormFile file, string folderName)
        {
            try
            {
                ValidateFile(file);
                ValidateFolder(folderName);

                using (var stream = file.OpenReadStream())
                {
                    string s3Key = GenerateS3Key(file.FileName, folderName);
                    await UploadToS3Async(stream, s3Key);

                    var request = new GetPreSignedUrlRequest
                    {
                        BucketName = _s3Settings.AwsBucketName,
                        Key = s3Key,
                        Expires = DateTime.UtcNow.AddMinutes(15)
                    };
                    string url = _s3Client.GetPreSignedURL(request);

                    return ApiResponse<object>.SuccessResponse(new { documentKey = s3Key, url = url }, "Document uploaded successfully");
                }
            }
            catch (ArgumentException ex)
            {
                return ApiResponse<object>.FailResponse(ex.Message, 400);
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.FailResponse($"Error uploading document: {ex.Message}", 500);
            }
        }


        public async Task<ApiResponse<object>> UploadBase64Async(string base64Data, string folderName, string fileName = "image.jpg")
        {
            try
            {
                if (string.IsNullOrEmpty(base64Data)) throw new ArgumentException("No data provided.");
                ValidateFolder(folderName);

                byte[] bytes = Convert.FromBase64String(base64Data);
                using (var stream = new MemoryStream(bytes))
                {
                    string s3Key = GenerateS3Key(fileName, folderName);
                    await UploadToS3Async(stream, s3Key);

                    var request = new GetPreSignedUrlRequest
                    {
                        BucketName = _s3Settings.AwsBucketName,
                        Key = s3Key,
                        Expires = DateTime.UtcNow.AddMinutes(15)
                    };
                    string url = _s3Client.GetPreSignedURL(request);

                    return ApiResponse<object>.SuccessResponse(new { documentKey = s3Key, url = url }, "Image uploaded successfully");
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.FailResponse($"Error uploading base64: {ex.Message}", 500);
            }
        }

        private async Task UploadToS3Async(Stream stream, string key)
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = key,
                BucketName = _s3Settings.AwsBucketName,
                AutoCloseStream = false // Handled by using blocks
            };

            var fileTransferUtility = new TransferUtility(_s3Client);
            await fileTransferUtility.UploadAsync(uploadRequest);
        }

        private string GenerateS3Key(string originalFileName, string folderName)
        {
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 6);
            return $"{folderName}/{timestamp}_{uniqueId}_{originalFileName}";
        }

        private void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");
        }

        private void ValidateFolder(string folderName)
        {
            var validFolders = new List<string> { "KYC", "logo", "user" };
            if (!validFolders.Contains(folderName))
                throw new ArgumentException("Invalid folder name. Allowed: KYC, logo, user.");
        }

        public async Task<ApiResponse<object>> GetDocumentAsync(string documentKey)
        {
            try
            {
                if (string.IsNullOrEmpty(documentKey))
                    throw new ArgumentException("Document key is required.");

                var request = new GetObjectRequest
                {
                    BucketName = _s3Settings.AwsBucketName,
                    Key = documentKey
                };

                using var response = await _s3Client.GetObjectAsync(request);
                using var reader = new MemoryStream();
                await response.ResponseStream.CopyToAsync(reader);

                return ApiResponse<object>.SuccessResponse(Convert.ToBase64String(reader.ToArray()), "Document retrieved successfully");
            }
            catch (ArgumentException ex)
            {
                return ApiResponse<object>.FailResponse(ex.Message, 400);
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.FailResponse($"Error retrieving document: {ex.Message}", 500);
            }
        }

        public ApiResponse<object> GetPreSignedUrl(string documentKey)
        {
            try
            {
                if (string.IsNullOrEmpty(documentKey))
                    throw new ArgumentException("Document key is required.");

                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _s3Settings.AwsBucketName,
                    Key = documentKey,
                    Expires = DateTime.UtcNow.AddMinutes(15)
                };

                string url = _s3Client.GetPreSignedURL(request);

                return ApiResponse<object>.SuccessResponse(new { url = url }, "Pre-signed URL generated successfully");
            }
            catch (ArgumentException ex)
            {
                return ApiResponse<object>.FailResponse(ex.Message, 400);
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.FailResponse($"Error generating pre-signed URL: {ex.Message}", 500);
            }
        }
    }
}
