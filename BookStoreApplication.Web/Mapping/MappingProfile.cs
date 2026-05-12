using AutoMapper;
using BookStoreApplication.Web.DTOs.Author;
using BookStoreApplication.Web.DTOs.Book_Publisher;
using BookStoreApplication.Web.DTOs.Category;
using BookStoreApplication.Web.DTOs.Inventory;
using BookStoreApplication.Web.Models;
using AuthorEntity = BookStoreApplication.Web.Models.Author;
using CategoryEntity = BookStoreApplication.Web.Models.Category;

namespace BookStoreApplication.Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookCreateDto, Book>()
                .ForMember(
                    dest => dest.Isbn,
                    opt => opt.MapFrom(src => src.Isbn.Trim()))
                .ForMember(
                    dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.Trim()))
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src =>
                        src.Description == null
                            ? null
                            : src.Description.Trim()))
                .ForMember(
                    dest => dest.Edition,
                    opt => opt.MapFrom(src =>
                        src.Edition == null
                            ? null
                            : src.Edition.Trim()))
                .ForMember(
                    dest => dest.Bookauthors,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Bookreviews,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.CategoryNavigation,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Inventories,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Publisher,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Shoppingcarts,
                    opt => opt.Ignore());

            CreateMap<BookUpdateDto, Book>()
                .ForMember(
                    dest => dest.Isbn,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title.Trim()))
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src =>
                        src.Description == null
                            ? null
                            : src.Description.Trim()))
                .ForMember(
                    dest => dest.Edition,
                    opt => opt.MapFrom(src =>
                        src.Edition == null
                            ? null
                            : src.Edition.Trim()))
                .ForMember(
                    dest => dest.Bookauthors,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Bookreviews,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.CategoryNavigation,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Inventories,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Publisher,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Shoppingcarts,
                    opt => opt.Ignore());

            CreateMap<Book, BookResponseDto>()
                .ForMember(
                    dest => dest.Isbn,
                    opt => opt.MapFrom(src => src.Isbn.Trim()))
                .ForMember(
                    dest => dest.Edition,
                    opt => opt.MapFrom(src =>
                        src.Edition == null
                            ? null
                            : src.Edition.Trim()))
                .ForMember(
                    dest => dest.PublisherName,
                    opt => opt.MapFrom(src =>
                        src.Publisher == null
                            ? null
                            : src.Publisher.Name));

            CreateMap<PublisherCreateDto, Publisher>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name.Trim()))
                .ForMember(
                    dest => dest.City,
                    opt => opt.MapFrom(src =>
                        src.City == null
                            ? null
                            : src.City.Trim()))
                .ForMember(
                    dest => dest.StateCode,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.StateCode)
                            ? null
                            : src.StateCode.Trim()))
                .ForMember(
                    dest => dest.Books,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.StateCodeNavigation,
                    opt => opt.Ignore());

            CreateMap<PublisherUpdateDto, Publisher>()
                .ForMember(
                    dest => dest.PublisherId,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name.Trim()))
                .ForMember(
                    dest => dest.City,
                    opt => opt.MapFrom(src =>
                        src.City == null
                            ? null
                            : src.City.Trim()))
                .ForMember(
                    dest => dest.StateCode,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.StateCode)
                            ? null
                            : src.StateCode.Trim()))
                .ForMember(
                    dest => dest.Books,
                    opt => opt.Ignore())
                .ForMember(
                    dest => dest.StateCodeNavigation,
                    opt => opt.Ignore());

            CreateMap<Publisher, PublisherResponseDto>()
                .ForMember(
                    dest => dest.StateCode,
                    opt => opt.MapFrom(src =>
                        src.StateCode == null
                            ? null
                            : src.StateCode.Trim()));

            CreateMap<Publisher, PublisherDetailsDto>()
                .ForMember(
                    dest => dest.StateCode,
                    opt => opt.MapFrom(src =>
                        src.StateCode == null
                            ? null
                            : src.StateCode.Trim()))
                .ForMember(
                    dest => dest.StateName,
                    opt => opt.MapFrom(src =>
                        src.StateCodeNavigation == null
                            ? null
                            : src.StateCodeNavigation.StateName))
                .ForMember(
                    dest => dest.Books,
                    opt => opt.MapFrom(src => src.Books));

            CreateMap<AuthorEntity, AuthorResponseDTO>();

            CreateMap<AuthorEntity, AuthorWithBooksResponseDTO>();

            CreateMap<CategoryRequestDto, CategoryEntity>()
                .ForMember(
                    dest => dest.CatDescription,
                    opt => opt.MapFrom(src => src.CatDescription));

            CreateMap<CategoryEntity, CategoryResponseDto>()
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
