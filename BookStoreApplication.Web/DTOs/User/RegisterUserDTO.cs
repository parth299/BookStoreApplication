using BookStoreApplication.Web.Models.Enums;

namespace BookStoreApplication.Web.DTOs.User
{
    public class RegisterUserDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Public registration defaults to User. Change roles only from backend/admin data.
        public int RoleNumber { get; set; } = (int)UserRole.User;
    }
}
