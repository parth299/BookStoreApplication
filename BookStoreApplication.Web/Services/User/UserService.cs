using AutoMapper;
using BookStoreApplication.Web.DTOs.Author;
using BookStoreApplication.Web.DTOs.User;
using BookStoreApplication.Web.Repositories.User;
using BookStoreApplication.Web.Services.Auth;
using BookStoreApplication.Web.Wrappers;
using Microsoft.AspNetCore.Identity;
using UserEntity = BookStoreApplication.Web.Models.User;

namespace BookStoreApplication.Web.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly PermRoleRepository _roleRepository;
        private readonly IJwtTokenService _jwtService;
        private readonly IMapper _mapper;

        public UserService(
            UserRepository userRepository,
            IPasswordHasher<UserEntity> passwordHasher,
            PermRoleRepository roleRepository,
            IJwtTokenService jwtService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<UserEntity>>> GetUsersByRoleId(int roleId)
        {
            var users = await _userRepository.GetUsersByRoleId(roleId);

            if (users == null || !users.Any())
            {
                return ApiResponse<List<UserEntity>>.NotFound(
                    $"No users found for role id {roleId}"
                );
            }

            return ApiResponse<List<UserEntity>>.SuccessResponse(
                users,
                "Users fetched successfully"
            );
        }

        public async Task<ApiResponse<List<UserEntity>>> GetUsersAsync()
        {
            var users = await _userRepository.GetAllUsers();

            if (users == null || !users.Any())
            {
                return ApiResponse<List<UserEntity>>.NotFound("No users found");
            }

            return ApiResponse<List<UserEntity>>.SuccessResponse(
                users,
                "Users fetched successfully"
            );
        }

        public async Task<ApiResponse<string>> ChangePasswordAsync(int id, string updatedPassword)
        {
            var existingUser = await _userRepository.GetByUserId(id);

            if (existingUser == null)
            {
                return ApiResponse<string>.NotFound(
                    $"User with id {id} not found"
                );
            }

            existingUser.Password = updatedPassword;
            existingUser.PasswordHash =
                _passwordHasher.HashPassword(existingUser, updatedPassword);

            await _userRepository.UpdatePassword(existingUser);

            return ApiResponse<string>.MessageResponse(
                "Password updated successfully"
            );
        }

        public async Task<ApiResponse<UserEntity>> GetUserProfileAsync(int id)
        {
            var user = await _userRepository.GetByUserId(id);

            if (user == null)
            {
                return ApiResponse<UserEntity>.NotFound(
                    $"User with id {id} not found"
                );
            }

            return ApiResponse<UserEntity>.SuccessResponse(
                user,
                "User profile fetched successfully"
            );
        }

        public async Task<ApiResponse<string>> RegisterAsync(RegisterUserDTO request)
        {
            if (request == null)
            {
                return ApiResponse<string>.BadRequest(
                    "Request body is required"
                );
            }

            var existingUser =
                await _userRepository.GetByUserNameAsync(request.UserName);

            if (existingUser != null)
            {
                return ApiResponse<string>.BadRequest(
                    $"User already exists with username {request.UserName}"
                );
            }

            var user = _mapper.Map<UserEntity>(request);

            user.PasswordHash =
                _passwordHasher.HashPassword(user, request.Password);

            user.Password = request.Password;

            await _userRepository.CreateUserAsync(user);

            return ApiResponse<string>.CreatedResponse(
                "User registered successfully"
            );
        }

        public async Task<ApiResponse<AuthResponseDTO>> LoginAsync(LoginUserDTO request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.UserName) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return ApiResponse<AuthResponseDTO>.BadRequest(
                    "Username and password are required"
                );
            }

            var existingUser =
                await _userRepository.GetByUserNameAsync(request.UserName);

            if (existingUser == null)
            {
                return ApiResponse<AuthResponseDTO>.NotFound(
                    "User not registered"
                );
            }

            var passwordResult =
                _passwordHasher.VerifyHashedPassword(
                    existingUser,
                    existingUser.PasswordHash,
                    request.Password
                );

            if (passwordResult != PasswordVerificationResult.Success)
            {
                return ApiResponse<AuthResponseDTO>.BadRequest(
                    "Incorrect password"
                );
            }

            var permRole =
                await _roleRepository.GetRoleById(existingUser.RoleNumber);

            if (permRole?.PermRole1 == null)
            {
                return ApiResponse<AuthResponseDTO>.BadRequest(
                    "Role not assigned"
                );
            }

            var token =
                await _jwtService.GenerateToken(existingUser, permRole.PermRole1);

            var authResponse = new AuthResponseDTO
            {
                Success = true,
                Message = "Login successful",
                Token = token
            };

            return ApiResponse<AuthResponseDTO>.SuccessResponse(
                authResponse,
                "Login successful"
            );
        }

        public async Task<ApiResponse<UserEntity>> UpdateUserAsync(UpdateUserDTO request)
        {
            if (request == null)
            {
                return ApiResponse<UserEntity>.BadRequest(
                    "User details are required"
                );
            }

            var existingUser =
                await _userRepository.GetByUserId(request.UserId);

            if (existingUser == null)
            {
                return ApiResponse<UserEntity>.NotFound(
                    $"User with id {request.UserId} not found"
                );
            }

            _mapper.Map(request, existingUser);

            await _userRepository.UpdateUser(existingUser);

            return ApiResponse<UserEntity>.SuccessResponse(
                existingUser,
                "User updated successfully"
            );
        }
    }
}