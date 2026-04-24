namespace ProfilePhotoService.Model.Models
{
    public class PhotoUploadRequest
    {
        public string Token { get; set; } = string.Empty;
        public string ImageBase64 { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}
