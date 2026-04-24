namespace ManageAccessService.Model.Models
{
    public class UserApiRequest
    {
        public string? UserId { get; set; }
        public string? MobileNo { get; set; }
        public string? UserName { get; set; }
        public List<string>? Vehicles { get; set; }
        public int AccessType { get; set; }
        public string AdminUserId { get; set; }
    }
}
