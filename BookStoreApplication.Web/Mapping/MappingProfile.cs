using AutoMapper;
using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.Models;

namespace BookStoreApplication.Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Author, AuthorResponseDTO>();

            CreateMap<Author, AuthorWithBooksResponseDTO>();

            CreateMap<CategoryRequestDto, Category>()
                .ForMember(
                    dest => dest.CatDescription,
                    opt => opt.MapFrom(src => src.CatDescription));

            CreateMap<Category, CategoryResponseDto>()
                .ForMember(
                    dest => dest.CatId,
                    opt => opt.MapFrom(src => src.CatId))
                .ForMember(
                    dest => dest.CatDescription,
                    opt => opt.MapFrom(src => src.CatDescription));


            CreateMap<Inventory, InventoryDto>()
                .ReverseMap();



            CreateMap<Shoppingcart, CartItemDto>()
                .ReverseMap();


          

            CreateMap<Purchaselog, PurchaseDto>()
                .ReverseMap();
        }
    }
}