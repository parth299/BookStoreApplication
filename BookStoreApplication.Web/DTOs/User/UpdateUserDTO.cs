using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApplication.Web.DTOs.User
{
    public class UpdateUserDTO
    {
        public int UserId { get; set; }

        public string LastName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string UserName { get; set; } = null!;
    }
}
