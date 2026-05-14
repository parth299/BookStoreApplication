namespace BookStoreApplication.MVC.DTOs.User
{
    public class UserResponseDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int RoleNumber { get; set; }
        public string RoleName { get; set; } = "User";
    }
}
