using BookStoreApplication.MVC.Models.Enums;

namespace BookStoreApplication.MVC.DTOs.User
{
    public class RegisterUserDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Public registration is always a normal User. Admin/Manager changes must be done from backend/admin flow.
        public int RoleNumber { get; set; } = (int)UserRole.User;
    }
}
