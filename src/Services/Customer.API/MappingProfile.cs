using AutoMapper;
using Shared.DTOs.CustomerDTOs;

namespace Customer.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Customer, CustomerDto>();
        }
    }
}
