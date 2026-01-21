using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Interfaces;
using BookAdvisor.Infrastructure.AI;
using BookAdvisor.Infrastructure.Messaging;
using BookAdvisor.Infrastructure.Persistence;
using BookAdvisor.Infrastructure.Persistence.Repositories;
using MassTransit;
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

            services.AddMassTransit(busConfigurrator =>
                {
                    busConfigurrator.SetKebabCaseEndpointNameFormatter();

                    busConfigurrator.AddConsumer<BookCreatedEventConsumer>();
                    busConfigurrator.UsingRabbitMq((context, cfg) =>
                    {
                        //Docker daki RabitMQ bilgileri
                        cfg.Host("localhost", "/", h =>
                        {
                            h.Username("guest");
                            h.Password("guest");
                        });
                        // Eğer Consumer hata alırsa (örn: Kitap DB'de henüz yoksa veya AI yanıt vermezse)
                        // 5 kez, her seferinde 1 saniye bekleyerek tekrar dene.
                        cfg.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(1)));
                        cfg.ConfigureEndpoints(context);
                    });
                }
            );


            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAiService, GeminiService>();


            return services;

        }
    }
}
