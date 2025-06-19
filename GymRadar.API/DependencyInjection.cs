using Appwrite;
using Appwrite.Services;
using GymRadar.Model.Entity;
using GymRadar.Model.Payload.Settings;
using GymRadar.Repository.Implement;
using GymRadar.Repository.Interface;
using GymRadar.Service.Implement;
using GymRadar.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GymRadar.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork<GymRadarContext>, UnitOfWork<GymRadarContext>>();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<GymRadarContext>(options => options.UseSqlServer(GetConnectionString()));
            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IGymService, GymService>();
            services.AddScoped<IPTService, PTService>();
            services.AddScoped<IGymCourseService, GymCourseService>();
            services.AddScoped<ISlotService, SlotService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPTSlotService, PTSlotService>();
            services.AddScoped<IGymCoursePTService, GymCoursePTService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<IPremiumService, PremiumService>();
            services.AddScoped<IDashboardService, DashboardService>();
            return services;
        }
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
        {
            services.AddHttpClient(); // Registers HttpClient
            return services;
        }

        public static IServiceCollection AddLazyResolution(this IServiceCollection services)
        {
            services.AddTransient(typeof(Lazy<>), typeof(LazyResolver<>));
            return services;
        }

        private class LazyResolver<T> : Lazy<T> where T : class
        {
            public LazyResolver(IServiceProvider serviceProvider)
                : base(() => serviceProvider.GetRequiredService<T>())
            {
            }
        }

        public static IServiceCollection AddJwtValidation(this IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = "GymRadar",
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Convert.FromHexString("0102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F00"))
                };
            });
            return services;
        }

        public static IServiceCollection AddAppwrite(this IServiceCollection services, IConfiguration configuration)
        {
            var appWriteSettings = configuration.GetSection("AppWrite").Get<AppWriteSettings>();
            if (appWriteSettings == null)
            {
                throw new ArgumentNullException(nameof(appWriteSettings), "AppWrite configuration is missing.");
            }

            services.AddSingleton(appWriteSettings);

            services.AddScoped<Client>(_ => new Client()
                .SetEndpoint(appWriteSettings.EndPoint)
                .SetProject(appWriteSettings.ProjectId)
                .SetKey(appWriteSettings.APIKey));

            services.AddScoped<Storage>(provider => new Storage(provider.GetRequiredService<Client>()));

            services.AddHttpClient<IUploadService, UploadService>();

            services.AddScoped<IUploadService, UploadService>();

            return services;
        }

        private static string GetConnectionString()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();
            var strConn = config["ConnectionStrings:DefautDB"];

            return strConn;
        }
    }
}
