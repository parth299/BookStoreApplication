using AutoMapper;
using BookStoreApplication.Web.DTOs.User;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Models.Enums;

namespace BookStoreApplication.Web.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserDTO, User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.RoleNumber, opt => opt.MapFrom(src => src.RoleNumber <= 0 ? (int)UserRole.User : src.RoleNumber))
                .ForMember(dest => dest.Purchaselogs, opt => opt.Ignore())
                .ForMember(dest => dest.RoleNumberNavigation, opt => opt.Ignore());

            CreateMap<UpdateUserDTO, User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Purchaselogs, opt => opt.Ignore())
                .ForMember(dest => dest.RoleNumberNavigation, opt => opt.Ignore());
        }
    }
}
