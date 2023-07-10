using AutoMapper;
using BusinessObjects.Models;
using ProjectManagementAPI.DTO;

namespace ProjectManagementAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Food, FoodDTO>().ReverseMap().ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<NewOrderDTO, Order>().ReverseMap();
            CreateMap<NewOrderDetailDTO, OrderDetail>().ReverseMap();
            CreateMap<NewUserDTO, User>().ReverseMap();
            CreateMap<NewCartDTO, Cart>().ReverseMap();
            CreateMap<Cart, CartDTO>().ReverseMap().ForMember(dest => dest.Food, opt => opt.MapFrom(src =>src.Food));
        }
    }
}
