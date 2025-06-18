using CloudinaryDotNet;
using Exchanger.API.Data;
using Exchanger.API.DTOs;
using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.DTOs.Cloudinary;
using Exchanger.API.DTOs.EmailSenderDTOs;
using Exchanger.API.Repositories;
using Exchanger.API.Repositories.IRepositories;
using Exchanger.API.Services;
using Exchanger.API.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Exchanger.API.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<ISessionTokenRepository, SessionTokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IListingRepository, ListingRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IListingService, ListingService>();
            services.AddScoped<IListingCategoryService, ListingCategoryService>();
            services.AddScoped<IListingImageService, ListingImageService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            services.AddSingleton<ICloudinaryClient, CloudinaryClient>();
            services.AddSingleton<IEmailSenderService, EmailSenderService>();

            services.Configure<JWTSettings>(
                configuration.GetSection("Jwt"));
            services.AddSingleton<JWTSettings>(provider =>
                provider.GetRequiredService<IOptions<JWTSettings>>().Value);

            services.Configure<AppSettings>(
                configuration.GetSection("AppSettings"));

            services.Configure<SmtpSettings>(
                configuration.GetSection("Smtp"));
            services.AddSingleton<SmtpSettings>(provider => 
                provider.GetRequiredService<IOptions<SmtpSettings>>().Value);

            services.Configure<CloudinarySettings>(
                configuration.GetSection("Cloudinary"));
            services.AddSingleton(provider =>
            {
                var settings = provider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
                var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
                return new Cloudinary(account);
            });
        }

        public static void AddCustomAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey("AccessToken"))
                        {
                            context.Token = context.Request.Cookies["AccessToken"];
                        }
                        return Task.CompletedTask;
                    }
                };

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding
                        .UTF8.GetBytes(configuration["Jwt:Key"])),
                };
            });
        }

        public static void AddCustomCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
        }
    }
}