using AutoMapper;
using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.DTOs;

namespace BookStoreApplication.Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Author mappings
            CreateMap<AuthorRequestDTO, Author>();
            CreateMap<Author, AuthorResponseDTO>();
            CreateMap<Author, AuthorWithBooksResponseDTO>();

            // Category mappings (Note: database uses CatID, CatDescription)
            CreateMap<CategoryRequestDto, Category>()
                .ForMember(dest => dest.CatDescription, opt => opt.MapFrom(src => src.CatDescription));
            CreateMap<Category, CategoryResponseDto>()
                .ForMember(dest => dest.CatId, opt => opt.MapFrom(src => src.CatId))
                .ForMember(dest => dest.CatDescription, opt => opt.MapFrom(src => src.CatDescription));
        }
    }
}