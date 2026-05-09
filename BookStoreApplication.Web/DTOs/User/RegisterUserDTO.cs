namespace BookStoreApplication.Web.DTOs.User
{
    public class RegisterUserDTO {
        public string FirstName {get; set;} = null!;
        
        public string LastName {get; set;} = null!;

        public string UserName {get; set;} = null!;

        public string PhoneNumber {get; set;} = null!;

        public string Password {get; set;} = null!;

        public int RoleNumber {get; set;}
    }
}