using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Interfaces;
using BookAdvisor.Infrastructure.AI;
using BookAdvisor.Infrastructure.Identity;
using BookAdvisor.Infrastructure.Messaging;
using BookAdvisor.Infrastructure.Persistence;
using BookAdvisor.Infrastructure.Persistence.Interceptors;
using BookAdvisor.Infrastructure.Persistence.Repositories;
using BookAdvisor.Infrastructure.Services;
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
            services.AddScoped<AuditableEntityInterceptor>();   //Interceptor DI kaydı
            //DbContext Kurulumu
            services.AddDbContext<BookAdvisorDbContext>((sp, opt) =>
            {
                var interseptor = sp.GetRequiredService<AuditableEntityInterceptor>();
                opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")).AddInterceptors(interseptor);
            }
            );

            //Identity Core Kurulumu
            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<BookAdvisorDbContext>();

            //MassTransit ve RabbitMQ Kurulumu
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

            //Servis Kayıtları
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IReadingListRepository, ReadingListRepository>();
            services.AddScoped<IAiService, GeminiService>();
            services.AddScoped<IAiKeyProvider, AiKeyProvider>();   //BYOK Servisi
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IReviewRepository, ReviewRepository>();


            return services;
        }
    }
}
