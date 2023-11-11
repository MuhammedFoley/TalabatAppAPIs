using Microsoft.AspNetCore.Mvc;
using Talabat.Repostory;
using TalabatAppAPIs.Erorrs;
using TalabatAppAPIs.Helpers;
using Talabt.Core.Repositories;

namespace TalabatAppAPIs.Extentions
{
    public static class ApicationServicesExtention
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositoryImpl<>));

            //builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            services.AddAutoMapper(typeof(MappingProfiles));
            services.Configure<ApiBehaviorOptions>(options =>
            {

                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var erorrs = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                    .SelectMany(p => p.Value.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray();




                    var validationErorrResponse = new ApiValidationErorrResponse()
                    {
                        Erorrs = erorrs
                    };

                    return new BadRequestObjectResult(validationErorrResponse);
                };
            });


            return services;
        }
    }
}
