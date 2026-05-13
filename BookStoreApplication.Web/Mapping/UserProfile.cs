using AutoMapper;
using BookStoreApplication.Web.DTOs.User;
using BookStoreApplication.Web.Models;

namespace BookStoreApplication.Web.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserDTO, User>();

            CreateMap<UpdateUserDTO, User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}