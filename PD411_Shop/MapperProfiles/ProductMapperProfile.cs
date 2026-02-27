using AutoMapper;
using PD411_Shop.Models;
using PD411_Shop.ViewModels;

namespace PD411_Shop.MapperProfiles
{
    public class ProductMapperProfile : Profile
    {
        public ProductMapperProfile()
        {
            // CreateProductVM -> ProductModel
            CreateMap<CreateProductVM, ProductModel>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            // ProductModel -> UpdateProductVM
            CreateMap<ProductModel, UpdateProductVM>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}
