using AutoMapper;
using TalabatAppAPIs.Dtos;
using Talabt.Core.Entities;

namespace TalabatAppAPIs.Helpers
{
    public class ProductPictureURLResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;
        public ProductPictureURLResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["ApiBaseURL"]}{ source.PictureUrl}";
            return string.Empty ;
        }
    }
}
