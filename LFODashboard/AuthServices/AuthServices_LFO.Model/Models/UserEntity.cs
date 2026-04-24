namespace AuthServices_LFO.Model.Models
{
    public class UserEntity
    {
        public Guid UserId { get; set; }
        public string MobileNo { get; set; }
        public string AccessType { get; set; }
        public bool IsActive { get; set; }
    }
}
