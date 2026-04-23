namespace ManageAccessService.Model.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string? MobileNo { get; set; }
        public string? AccessType { get; set; }
        public bool IsActive { get; set; }
    }
}
