using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BookAdvisor.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Bu satır, bu assembly (proje) içindeki tüm Command/Query Handler'ları bulup otomatik register eder.
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}
