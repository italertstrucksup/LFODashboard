using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using MediaService.BL.Model;
using MediaService.BL.IBussinessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text;
using Amazon.S3.Model;
using MediaService.Model.Model;

namespace MediaServiceAPI.BussinessLayer
{
    public class  MediaBusinessLayer : IMediaBusinessLayer
    {
        private readonly S3Settings _s3Settings;
        private readonly IAmazonS3 _s3Client;

        public MediaBusinessLayer(IOptions<S3Settings> s3Settings)
        {
            _s3Settings = s3Settings.Value ?? throw new ArgumentNullException(nameof(s3Settings));

            if (!string.IsNullOrEmpty(_s3Settings.AwsAccessKeyId) && !string.IsNullOrEmpty(_s3Settings.AwsSecretKey))
            {
                _s3Client = new AmazonS3Client(_s3Settings.AwsAccessKeyId, _s3Settings.AwsSecretKey, RegionEndpoint.APSouth1);
            }
            else
            {
                _s3Client = new AmazonS3Client(RegionEndpoint.APSouth1);
            }
        }

        public async Task<MediaResponse> UploadDocumentAsync(IFormFile file, string folderName)
        {
            try
            {
                ValidateFile(file);
                ValidateFolder(folderName);
                using var newMemoryStream = new MemoryStream();
                await file.CopyToAsync(newMemoryStream);

                var fileTransferUtility = new TransferUtility(_s3Client);

                string preFixKey = GenerateUniquePrefix(5);
                string uniqueFileName = $"{preFixKey}_{file.FileName}";
                string s3Key = $"{uniqueFileName}";

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = s3Key,
                    BucketName = _s3Settings.AwsBucketName
                };

                await fileTransferUtility.UploadAsync(uploadRequest);

                return new MediaResponse
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Document uploaded successfully",
                    Data = new { documentKey = s3Key }
                };
            }
            catch (ArgumentException ex)
            {
                return new MediaResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new MediaResponse
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = $"Error uploading document: {ex.Message}"
                };
            }
        }

        

        private string GenerateUniquePrefix(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
                result.Append(chars[random.Next(chars.Length)]);

            return result.ToString() + DateTime.Now.ToString("ddMMyyyyHHmmssfff");
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


        public async Task<MediaResponse> GetDocumentAsync(string documentKey)
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
                using var responseStream = response.ResponseStream;
                using var reader = new MemoryStream();
                await responseStream.CopyToAsync(reader);

                return new MediaResponse
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Document retrieved successfully",
                    Data = Convert.ToBase64String(reader.ToArray())
                };
            }
            catch (ArgumentException ex)
            {
                return new MediaResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new MediaResponse
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = $"Error retrieving document: {ex.Message}"
                };
            }
        }

        public MediaResponse GetPreSignedUrl(string documentKey)
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

                return new MediaResponse
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Pre-signed URL generated successfully",
                    Data = new { url = url }
                };
            }
            catch (ArgumentException ex)
            {
                return new MediaResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new MediaResponse
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = $"Error generating pre-signed URL: {ex.Message}"
                };
            }
        }

        //public async Task<bool> DeleteDocumentAsync(string documentKey)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(documentKey))
        //            throw new ArgumentException("Document key is required.");

        //        var deleteObjectRequest = new DeleteObjectRequest
        //        {
        //            BucketName = _s3Settings.AwsBucketName,
        //            Key = documentKey
        //        };

        //        await _s3Client.DeleteObjectAsync(deleteObjectRequest);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error deleting document from S3: {ex.Message}");
        //    }
        //}
    }
}
