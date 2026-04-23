using System;
using System.Collections.Generic;
using System.Text;

namespace ProfilePhotoService.Model.Models
{
    public class MediaUploadRequest
    {
        public string File { get; set; } = string.Empty; // Holds Base64 data
        public string FolderName { get; set; } = string.Empty;
    }
}
