using APP.Repository.EFRepo;
using APP.Repository.EFRepo.UnitOfWork;
using APP.Services.Email;
using Microsoft.Extensions.DependencyInjection;

namespace APP.API.Utilities
{
    public static class DIContainer
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork>(config => new UnitOfWork(config.GetRequiredService<AppDBContext>()));
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
