namespace ProfilePhotoService.Model.Models
{
    public class UploadStatusResponse
    {
        public bool IsUploaded { get; set; }
        public string? ImageUrl { get; set; }
        public string StatusMessage { get; set; } = "Awaiting upload...";
        public DateTime? UploadedAt { get; set; }
    }
}
