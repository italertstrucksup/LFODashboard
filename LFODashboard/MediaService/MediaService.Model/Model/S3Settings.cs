namespace MediaService.BL.Model
{
    public class S3Settings
    {
        public string AwsAccessKeyId { get; set; } = string.Empty;
        public string AwsSecretKey { get; set; } = string.Empty;
        public string AwsBucketName { get; set; } = string.Empty;
    }
}
        