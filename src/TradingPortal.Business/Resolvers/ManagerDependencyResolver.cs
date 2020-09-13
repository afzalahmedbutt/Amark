using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TradingPortal.Business.interfaces;
using TradingPortal.Core.Caching;
using TradingPortal.Core.Domain.Common;
using TradingPortal.Core.Domain.Orders;
using TradingPortal.Core.ViewModels;
using TradingPortal.Infrastructure.Services;
using TradingPortal.Infrastructure.Services.Interfaces;

namespace TradingPortal.Business.Resolvers
{
    public static class ManagerResolvers
    {
        public static void AddManagers(this IServiceCollection services)
        {
            services.AddScoped<IEmailManager, EmailManager>();
            services.AddScoped<IGenericAttributeManager, GenericAttributeManager>();
            services.AddScoped<IShoppingCartManager,ShoppingCartManager>();
            services.AddScoped<ISpotPriceManager, SpotPriceManager>();
            services.AddScoped<ITopicManager, TopicManager>();
            services.AddScoped<ITopicManager, TopicManager>();
            services.AddScoped<ICheckoutAttributeParser, CheckoutAttributeParser>();
            services.AddScoped<ICheckoutAttributeParser, CheckoutAttributeParser>();
            services.AddScoped<ICustomerManager, CustomerManager>();
            //services.AddScoped<ICheckoutAttributeManager, CheckoutAttributeManager>();
            services.AddScoped<IProductAttributeService, ProductAttributeService>();
            services.AddScoped<IProductAttributeParser, ProductAttributeParser>();
            services.AddScoped<ICacheManager, CacheManager>();
            services.AddScoped<ISettingsManager, SettingsManager>();
            services.AddScoped<IRequestForOrderManager, RequestForOrderManager>();
            services.AddScoped<IExportManager, ExportManager>();
            services.AddScoped<IFeedsManager, FeedsManager>();
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<IContentManager, ContentManager>();
            services.AddScoped<IAccountManager, AccountManager>();
            services.AddScoped<IContactUsManager, ContactUsManager>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IStoreManager, StoreManager>();
        }

        public static void AddSettings(this IServiceCollection services)
        {
            services.AddScoped<CustomerAttributes, CustomerAttributes>((ctx) =>
             {
                 var _currentUser = ctx.GetService<ICurrentUser>();
                 var _genericAttributeManager = ctx.GetService<IGenericAttributeManager>();
                 var customerAttributes = _genericAttributeManager.GetCustomerAttributes(_currentUser.User.Id).Result;
                 return customerAttributes;
             });
            services.AddScoped<ShoppingCartSettings, ShoppingCartSettings>((ctx) =>
            {
                var _settingsService = ctx.GetService<ISettingsService>();
                return _settingsService.LoadSetting<ShoppingCartSettings>(1);
            });
            services.AddScoped<CustomSettings, CustomSettings>((ctx) =>
            {
                var _settingsService = ctx.GetService<ISettingsService>();
                return _settingsService.LoadSetting<CustomSettings>(1);
            });
            services.AddScoped<OrderSettings, OrderSettings>((ctx) =>
            {
                var _settingsService = ctx.GetService<ISettingsService>();
                return _settingsService.LoadSetting<OrderSettings>(1);
            });
        }
    }
}
