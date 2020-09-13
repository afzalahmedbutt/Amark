
using System;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using TradingPortal.Core.Infrastructure;
using TradingPortal.Infrastructure.DatabaseContexts;
//using TradingPortal.Web.Authorization;
using AppPermissions = TradingPortal.Core.Infrastructure.ApplicationPermissions;
using TradingPortal.Infrastructure.Configs;
using TradingPortal.Infrastructure.Services;
using TradingPortal.Infrastructure.IdentityExtensions;
using TradingPortal.Core.Domain.Identity;
using TradingPortal.Infrastructure.Repositories;
using TradingPortal.Business;
using TradingPortal.Business.Resolvers;
using TradingPortal.Web.Authorization;
using Microsoft.AspNetCore.Http;
using NLog;
using System.IO;
using NLog.Extensions.Logging;
using TradingPortal.Infrastructure.Services.Interfaces;
using TradingPortal.Infrastructure.Extensions;
using AutoMapper;
using System.Collections.Generic;

namespace TradingPortal.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        //public Dictionary<string,string> AppSettings { get; set; }

        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
            //Configuration.GetSection("MobileConfigInfo").Bind(AppSettings);
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddHostedService();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"], b => b.MigrationsAssembly("TradingPortal.Web").UseRowNumberForPaging());
                options.UseOpenIddict();
            });
            services.AddDbContext<AMarkDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:AmarkEntities"]);
            });
            services.AddScoped<IPasswordHasher<Customer>, CustomPasswordHasher<Customer>>();

            // add identity
            services.AddIdentity<Customer, Role>()
                .AddUserStore<CustomUserStore>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure Identity options and password complexity here
            services.Configure<IdentityOptions>(options =>
            {
                // User settings
                options.User.RequireUniqueEmail = true;

                //    //// Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                //    //// Lockout settings
                //    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                //    //options.Lockout.MaxFailedAccessAttempts = 10;

                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });
            services.Configure<CustomSection>((settings) =>
            {
                Configuration.GetSection("AppSettings").Bind(settings);
            });

            services.AddOtherServices();
            services.AddRepositoryConfig();
            services.AddManagers();
            services.AddSettings();
            
            
            // Register the OpenIddict services.
            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore().UseDbContext<ApplicationDbContext>();
                })
                .AddServer(options =>
                {
                    options.UseMvc();
                    options.EnableTokenEndpoint("/connect/token");
                    options.AllowPasswordFlow();
                    options.AllowRefreshTokenFlow()
                    .SetAccessTokenLifetime(TimeSpan.FromMinutes(400));
                    options.AcceptAnonymousClients();
                    options.DisableHttpsRequirement(); // Note: Comment this out in production
                    options.RegisterScopes(
                        OpenIdConnectConstants.Scopes.OpenId,
                        OpenIdConnectConstants.Scopes.Email,
                        OpenIdConnectConstants.Scopes.Phone,
                        OpenIdConnectConstants.Scopes.Profile,
                        OpenIdConnectConstants.Scopes.OfflineAccess,
                        OpenIddictConstants.Scopes.Roles);

                    // options.UseRollingTokens(); //Uncomment to renew refresh tokens on every refreshToken request
                    // Note: to use JWT access tokens instead of the default encrypted format, the following lines are required:
                    // options.UseJsonWebTokens();
                })
                .AddValidation(); //Only compatible with the default token format. For JWT tokens, use the Microsoft JWT bearer handler.



            // Add cors
            services.AddCors();

            // Add framework services.
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });


            //Todo: ***Using DataAnnotations for validation until Swashbuckle supports FluentValidation***
            //services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.ViewAllUsersPolicy, policy => policy.RequireClaim(CustomClaimTypes.Permission, AppPermissions.ViewUsers));
                options.AddPolicy(Policies.ManageAllUsersPolicy, policy => policy.RequireClaim(CustomClaimTypes.Permission, AppPermissions.ManageUsers));

                options.AddPolicy(Policies.ViewAllRolesPolicy, policy => policy.RequireClaim(CustomClaimTypes.Permission, AppPermissions.ViewRoles));
                options.AddPolicy(Policies.ViewRoleByRoleNamePolicy, policy => policy.Requirements.Add(new ViewRoleAuthorizationRequirement()));
                options.AddPolicy(Policies.ManageAllRolesPolicy, policy => policy.RequireClaim(CustomClaimTypes.Permission, AppPermissions.ManageRoles));

                options.AddPolicy(Policies.AssignAllowedRolesPolicy, policy => policy.Requirements.Add(new AssignRolesAuthorizationRequirement()));
            });

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = ".AMark.Session";
                //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });


            // Auth Handlers
            services.AddSingleton<IAuthorizationHandler, ViewUserAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ManageUserAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ViewRoleAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, AssignRolesAuthorizationHandler>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddSingleton<IConfiguration>(Configuration);
            // DB Creation and Seeding
            //services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug(LogLevel.Warning);
            //loggerFactory.AddFile("Logs/ErrorLogs-{Date}.txt", LogLevel.Error, isJson: true);
            GlobalDiagnosticsContext.Set("configDir", "C:\\AMarkTradingPortal\\Logs");
            GlobalDiagnosticsContext.Set("connectionString", Configuration.GetConnectionString("DefaultConnection"));
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                //app.ConfigureCustomExceptionMiddleware();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.ConfigureCustomExceptionMiddleware();
   
            }

            app.Use(async (context, next) =>
            {
                var c = context;
                await next.Invoke();

                if (context.Response.StatusCode == StatusCodes.Status404NotFound
                || context.Response.StatusCode == StatusCodes.Status401Unauthorized
                || context.Response.StatusCode == StatusCodes.Status400BadRequest)
                {
                    await context.Response.WriteAsync("You seem to have mistyped the url");
                }
            });

            //Configure Cors
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSession();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                    spa.Options.StartupTimeout = TimeSpan.FromSeconds(60); // Increase the timeout if angular app is taking longer to startup
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:4200"); // Use this instead to use the angular cli server
                }
            });
        }
    }
}





















