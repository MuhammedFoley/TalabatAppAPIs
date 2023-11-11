using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Repostory.identity;
using Talabt.Core.Entities.identity;
using Talabt.Core.Services;
using Talabte.Service;

namespace TalabatAppAPIs.Extentions
{
    public static class IdentityServiceExtentions
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddScoped<ITokenService, TokenService>();

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                //options.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<AppIdentityDbContext>();
            //اللي بين الاقواس دا بعملة لو انا  عامل توكن اما قبل التوكن كانو فاضيين اساسا ود عبارة عن الاسطيما بتعتي
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;

                //option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                //ودي معاها
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                       ValidateIssuer = true,
                       ValidIssuer = configuration["JWT:ValidIssuer"],
                       ValidateAudience = true,
                       ValidAudience= configuration["JWT:ValidAudiance"],
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))

                };
                });
            return services;
        }
    }
}
