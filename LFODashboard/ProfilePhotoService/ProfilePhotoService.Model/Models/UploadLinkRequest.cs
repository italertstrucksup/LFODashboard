namespace ProfilePhotoService.Model.Models
{
    public class UploadLinkRequest
    {
        public Guid LoginId { get; set; }
        public string MobileNo { get; set; } = string.Empty;
        //public string RequestSource { get; set; } = "WEB"; // SMS or WhatsApp
        public string PhotoType { get; set; } = "profile"; // "profile" or "live"
    }
}
