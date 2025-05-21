using AutoMapper;
using Product.API.Entities;
using Shared.DTOs.ProductDTOs;

namespace Product.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CatalogProduct, ProductDTO>();
        CreateMap<CreateProductDTO, CatalogProduct>();
        CreateMap<UpdateProductDTO, CatalogProduct>();
    }
}
