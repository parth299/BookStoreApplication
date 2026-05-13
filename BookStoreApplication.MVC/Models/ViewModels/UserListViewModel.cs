using System.Collections.Generic;

namespace BookStoreApplication.MVC.Models
{
    public class UserListViewModel
    {
        public List<UserDto> Users { get; set; } = new List<UserDto>();
    }
}
