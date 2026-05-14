using BookStoreApplication.MVC.DTOs.User;

namespace BookStoreApplication.MVC.Models
{
    public class UserListViewModel
    {
        public List<UserResponseDTO> Users { get; set; } = new();
    }
}
