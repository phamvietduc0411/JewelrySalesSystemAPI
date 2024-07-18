namespace JewelrySalesSystem.BAL.Models.Users
{
    public class GetUserResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Role {  get; set; } = string.Empty;
        public string Counter { get; set; } = string.Empty;
    }
}
