namespace MediaService.Model.Model
{
    public class MediaUploadRequest
    {
        public string File { get; set; } = string.Empty; // Holds Base64 data
        public string FolderName { get; set; } = string.Empty;
    }
}
