namespace TalabatAppAPIs.Extentions
{
    public static class SwaggerServicesExtentions
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            return services;
        }


        public static WebApplication useSwagger(this WebApplication app) {

            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
