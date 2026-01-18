using BookAdvisor.Domain.Interfaces;
using BookAdvisor.Infrastructure.Persistence;
using BookAdvisor.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookAdvisor.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookAdvisorDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IBookRepository, BookRepository>();
            return services;
        }
    }
}
