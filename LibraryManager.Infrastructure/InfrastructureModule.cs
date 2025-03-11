using LibraryManager.Core.Interfaces;
using LibraryManager.Infrastructure.Auth;
using LibraryManager.Infrastructure.Notifications;
using LibraryManager.Infrastructure.Persistence;
using LibraryManager.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SendGrid.Extensions.DependencyInjection;
using System.Text;

namespace LibraryManager.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddRepositories()
                .AddData(configuration)
                .AddAuth(configuration)
                .AddEmailService(configuration);

            return services;
        }

        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("LibraryManagerCs");

            services.AddDbContext<LibraryManagerDbContext>(o => o.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IloanRepository, LoanRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });

            return services;
        }

        private static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddScoped<IEmailService, MailgunEmailService>();

            return services;
        }
    }
}
