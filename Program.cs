
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.Repostory;
using Talabat.Repostory.Data;
using Talabat.Repostory.identity;
using TalabatAppAPIs.Erorrs;
using TalabatAppAPIs.Extentions;
using TalabatAppAPIs.Helpers;
using TalabatAppAPIs.Middlewares;
using Talabt.Core.Entities.identity;
using Talabt.Core.Repositories;

namespace TalabatAppAPIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region configuration
            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerServices();

            builder.Services.AddDbContext<StoreContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));


            //
            builder.Services.AddSingleton<IConnectionMultiplexer>(S =>
            {
                var connetionStr = builder.Configuration.GetConnectionString("Redis");

                return ConnectionMultiplexer.Connect(connetionStr);
            }); 
            builder.Services.AddApplicationServices();


            builder.Services.AddIdentityService(builder.Configuration);
            #endregion


             var app = builder.Build();

            #region make the migration applayed automaticlly
            using var scope=app.Services.CreateScope();
            var service=scope.ServiceProvider;
            var loggerFactory=service.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbcontext = service.GetRequiredService<StoreContext>();
                await dbcontext.Database.MigrateAsync();

                 await StoreContextSeed.seeAsync(dbcontext);

                var identity = service.GetRequiredService<AppIdentityDbContext>();
                await identity.Database.MigrateAsync();
                //to make seeding to users data
                var useManger=service.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContectSeed.seedUserAsync(useManger);

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an erorr occured when applaying migration Foley");

            }

            #endregion

            #region configur kestrel middleWare
            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                app.useSwagger();
            }

            app.UseStatusCodePagesWithRedirects("/erorrs/{0}");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapControllers();
            app.UseAuthentication();
            app.UseAuthorization();
            #endregion

            app.Run();
        }
    }
}