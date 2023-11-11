using AutoMapper;
using TalabatAppAPIs.Dtos;
using Talabt.Core.Entities;
using Talabt.Core.Entities.identity;

namespace TalabatAppAPIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d=>d.ProductBrand,o =>o.MapFrom(s=>s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductPictureURLResolver>());

            CreateMap<Adress, AdressDTO>().ReverseMap();
        }
    }
}
