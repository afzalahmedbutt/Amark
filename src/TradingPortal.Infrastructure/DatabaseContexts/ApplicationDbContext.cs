using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TradingPortal.Core.Domain;
using TradingPortal.Core.Domain.Common;
using TradingPortal.Core.Domain.Configuration;
using TradingPortal.Core.Domain.Identity;
using TradingPortal.Core.ViewModels;
using TradingPortal.Infrastructure.ComplexTypes;
using TradingPortal.Infrastructure.EntityConfigurations;
using TradingPortal.Infrastructure.EntityConfigurations.Common;

namespace TradingPortal.Infrastructure.DatabaseContexts
{
    //public class ApplicationDbContext : IdentityDbContext<Customer, Role, int>
    public class ApplicationDbContext : IdentityDbContext<
        Customer, Role, int,
        IdentityUserClaim<int>, CustomerRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        private readonly IHttpContextAccessor _context;
        private Customer _currentUser;
        public string CurrentUserId { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor context = null)
            : base(options)
        {
            _context = context;
        }
        public DbSet<GenericAttribute> GenericAttributes { get; set; }
        public DbSet<Product> Products { get; set; }
        //public DbSet<MessageTemplate> MessageTemplates { get; set; }
        public DbSet<EmailAccount> EmailAccounts { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<PermissionRecord> PermissionRecords { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<CustomerFavoriteProduct> CustomerFavoriteProducts { get; set; }
        public DbSet<MessageTemplate> MessageTemplates { get; set; }
        //public DbSet<Role> Roles { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new CustomerMap());
            builder.ApplyConfiguration(new RoleMap());
            builder.ApplyConfiguration(new CustomerRoleMap());
            builder.ApplyConfiguration(new GenericAttributeMap());
            builder.ApplyConfiguration(new ProductMap());
            builder.ApplyConfiguration(new EmailAccountMap());
            builder.ApplyConfiguration(new TopicMap());
            builder.ApplyConfiguration(new ShoppingCartItemMap());
            builder.ApplyConfiguration(new PermissionRecordMap());
            builder.ApplyConfiguration(new PermissionRecordRoleMap());
            builder.ApplyConfiguration(new SettingsMap());
            builder.ApplyConfiguration(new CustomerFavoriteProductMap());
            builder.ApplyConfiguration(new MessageTemplateMap());
            builder.ApplyConfiguration(new VendorMap());

            builder.Entity<TradingPortal.Core.Domain.Amark.ContactUsMessage>().HasKey(cum => cum.Id);


        }

        internal Customer CurrentUser
        {
            get
            {
                if (_currentUser != null)
                    return _currentUser;
                var name = _context.HttpContext.User?.Identity?.Name;
                if(name != null)
                _currentUser = this.Users.Include(u => u.ShoppingCartItems).ThenInclude(sci => sci.Product).FirstOrDefault(u => u.Email == name);
                return _currentUser;

            }
        }

    }
}
