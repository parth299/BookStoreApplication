using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Repositories;
using Microsoft.AspNetCore.Identity;

namespace BookStoreApplication.Web.services
{
    public class AuthService: IAuthService
    {
        private readonly UserReporitory _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(UserReporitory userReporitory, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userReporitory;
            _passwordHasher = passwordHasher;
        }

        public async Task<string?> LoginAsync(LoginUserDTO request)
        {
            if(request is null)
            {
                return "Provide username and password";
            }

            var existing_user = await _userRepository.GetByUserNameAsync(request.UserName);

            // User need to register first;
            if(existing_user is null)
            {
                return null!;
            }

            // Verify the password and generate the token
            var result = _passwordHasher.VerifyHashedPassword(existing_user, existing_user.PasswordHash, request.Password);

            if(result == PasswordVerificationResult.Success)
            {
                return "true";
            }

            return "Incorrect Password";
        }

        public async Task<RegisterUserDTO?> RegisterAsync(RegisterUserDTO request) {
            if(request is null) {
                return null!;
            } 

            var existing_user = await _userRepository.GetByUserNameAsync(request.UserName);

            if(existing_user is not null)
            {
                return null!;
            }

            // Register the user
            var user = new User();

            user.UserName = request.UserName;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.RoleNumber = request.RoleNumber;
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            user.Password = request.Password;

            await _userRepository.CreateUserAsync(user);
            
            return request;
        }   
    }
}