using Microsoft.Extensions.DependencyInjection;
using TradingPortal.Core;
using TradingPortal.Infrastructure.Repositories;
using TradingPortal.Infrastructure.Repositories.Interfaces;
using TradingPortal.Infrastructure.Services;
using TradingPortal.Infrastructure.Services.Interfaces;
using UO = TradingPortal.Infrastructure.UnitOfWork;

namespace TradingPortal.Infrastructure.Configs
{
    public static class ConfigurationExtenstions
    {

        public static void AddRepositoryConfig(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IFeedsRepository, FeedsRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<ICurrentUser,CurrentUser>();
            services.AddScoped(typeof(IAMarkRepository<>), typeof(EFAMarkRespository<>));
            services.AddScoped<IStoreCommandRepository,StoreCommandRepository>();
            services.AddScoped<IAmarkProductRepository, AmarkProductRepository>();
            services.AddScoped<IContentRepository, ContentRepository>();
        }

        public static void AddOtherServices(this IServiceCollection services)
        {
            services.AddScoped<UO.IUnitOfWork,UO.UnitOfWork>();
            services.AddTransient<IEncryptionService, EncryptionService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<StoreClosedAttribute>();


        }
    }
}
