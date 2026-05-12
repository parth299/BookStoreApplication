using AutoMapper;
using BookStoreApplication.Web.DTOs.Author;
using BookStoreApplication.Web.DTOs.User;
using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Repositories.User;
using BookStoreApplication.Web.Services.Auth;
using Microsoft.AspNetCore.Identity;
using UserEntity = BookStoreApplication.Web.Models.User;

namespace BookStoreApplication.Web.Services.User
{
    public class UserService: IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly PermRoleRepository _roleRepository;
        private readonly IJwtTokenService _jwtService;
        private readonly IMapper _mapper;

        public UserService(UserRepository userReporitory, IPasswordHasher<UserEntity> passwordHasher, PermRoleRepository roleRepository, IJwtTokenService jwtService, IMapper mapper)
        {
            _userRepository = userReporitory;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<UserEntity?> ChangePasswordAsync(int id, string updated_password)
        {
            var existing_user = await _userRepository.GetByUserId(id);

            if(existing_user is null)
            {
                throw new NotFoundException($"User with id: {id} does not exists");
            }

            existing_user.Password = updated_password;
            existing_user.PasswordHash = _passwordHasher.HashPassword(existing_user, updated_password);

            await _userRepository.UpdatePassword(existing_user);

            return existing_user;
        }

        public async Task<List<UserEntity>> GetUsersByRoleId(int roleId)
        {
            var users = await _userRepository.GetUsersByRoleId(roleId);

            return users;
        }

        public async Task<UserEntity?> UpdateUserAsync(UpdateUserDTO request)
        {
            if(request is null)
            {
                throw new BadRequestException($"User details Incomplete :: Provide complete user details for updation");
            }

            var existing_user = await _userRepository.GetByUserId(request.UserId);
            if(existing_user is null)
            {
                throw new NotFoundException($"User Not Found :: Username : {request.UserName}");
            }

            _mapper.Map(request, existing_user);

            await _userRepository.UpdateUser(existing_user);

            return existing_user;
        }

        public async Task<UserEntity?> GetUserProfileAsync(int id)
        {
            return await _userRepository.GetByUserId(id);
        }

        public async Task<List<UserEntity>> GetUsersAsync()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginUserDTO request)
        {
            if (request is null ||
                string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = "Provide username and password"
                };
            }

            var existingUser = await _userRepository.GetByUserNameAsync(request.UserName);

            if (existingUser is null)
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = "User not registered"
                };
            }

            var result = _passwordHasher.VerifyHashedPassword(
                existingUser,
                existingUser.PasswordHash,
                request.Password
            );

            if (result != PasswordVerificationResult.Success)
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = "Incorrect password"
                };
            }

            var permRole = await _roleRepository.GetRoleById(existingUser.RoleNumber);

            if (permRole?.PermRole1 is null)
            {
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = "Role not assigned"
                };
            }

            var token = await _jwtService.GenerateToken(existingUser, permRole.PermRole1);

            return new AuthResponseDTO
            {
                Success = true,
                Message = "Login successful",
                Token = token
            };
        }

        public async Task<RegisterUserDTO?> RegisterAsync(RegisterUserDTO request) {
            if(request is null) {
                throw new BadRequestException($"Incomplete User Details :: Provide complete request body");
            } 

            var existing_user = await _userRepository.GetByUserNameAsync(request.UserName);

            if(existing_user is not null)
            {
                throw new BadRequestException($"User Already Exists With Username : {request.UserName}");
            }

            // Register the user
            var user = _mapper.Map<UserEntity>(request);

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            user.Password = request.Password;   

            await _userRepository.CreateUserAsync(user);
            
            return request;
        }   
    }
}
