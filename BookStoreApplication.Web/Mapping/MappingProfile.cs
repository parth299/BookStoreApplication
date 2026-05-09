using AutoMapper;
using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.Models;

namespace BookStoreApplication.Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Inventory, InventoryDto>().ReverseMap();

            CreateMap<Shoppingcart, CartItemDto>().ReverseMap();

            CreateMap<Purchaselog, PurchaseDto>().ReverseMap();
        }
    }
}
