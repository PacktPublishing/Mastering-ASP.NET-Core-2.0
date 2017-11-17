using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookShop.Web.Data;
using BookShop.Web.Models;
using BookShop.Web.Services;
using BookShop.DomainModel;
using BookShop.WebComponents.ValueProviders;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Localization.Routing;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using BookShop.WebComponents.Constraints;
using Microsoft.AspNetCore.Mvc.Localization;
using BookShop.Web.Hubs;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using System;
using BookShop.WebComponents;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using BookShop.WebComponents.Logging;
using BookShop.WebComponents.Theming;

namespace BookShop.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.Configuration.GetReloadToken().RegisterChangeCallback((state) =>
            {
                //will be called whenever the configuration changes
            }, null);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.AddDbContext<BookShopContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("BookShop")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("BookShopAuth")));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 0;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddSession();

            services.AddElm();

            services.AddSignalR();

            services
                .AddMvc(options =>
                {
                    options.CacheProfiles.Add("Public5Minutes", new CacheProfile { Duration = 5 * 60, Location = ResponseCacheLocation.Any, VaryByHeader = "Accept-Language" });
                    options.CacheProfiles.Add("Public1Hour", new CacheProfile { Duration = 60 * 60, Location = ResponseCacheLocation.Any, VaryByHeader = "Accept-Language" });

                    options.ValueProviderFactories.Add(new CookieValueProviderFactory());

                    options.Filters.Add(new MiddlewareFilterAttribute(typeof(LocalizationPipeline)));
                })
                .AddViewLocalization(options =>
                {
                    options.ResourcesPath = "Resources";
                })
                .AddDataAnnotationsLocalization(options =>
                {
                })
                .AddViewOptions(options =>
                {
                    options.HtmlHelperOptions.ClientValidationEnabled = true;
                })
                .AddRazorOptions(options =>
                {
                    options.ViewLocationExpanders.Add(new ThemeViewLocationExpander("MyTheme"));
                })
                .AddRazorPagesOptions(options =>
                {
                });

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add(CultureRouteConstraint.CultureKey, typeof(CultureRouteConstraint));
            });

            var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("pt"),
                    new CultureInfo("en")
                };

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(supportedCultures.First().Name, supportedCultures.First().Name);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new[] { new RouteDataRequestCultureProvider { Options = options } };
            });

            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<RequestLocalizationOptions> requestLocalizationOptions, IServiceProvider serviceProvider, DiagnosticListener diagnosticListener)
        {
            //loggerFactory.AddFile((categoryName, logLevel) => true);

            diagnosticListener.SubscribeWithAdapter(new BookListener());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();
            app.UseMiddleware<LoggingMiddleware>();
            app.UseRequestLocalization(requestLocalizationOptions.Value);
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseElmPage();
            app.UseElmCapture();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("chat", opt =>
                {
                    opt.Transports = Microsoft.AspNetCore.Sockets.TransportType.LongPolling | Microsoft.AspNetCore.Sockets.TransportType.ServerSentEvents;
                });
                routes.MapHub<TimerHub>("timer", opt =>
                {
                    opt.Transports = Microsoft.AspNetCore.Sockets.TransportType.LongPolling | Microsoft.AspNetCore.Sockets.TransportType.ServerSentEvents;
                });
            });

            TimerCallback callback = (x) => {
                var hub = serviceProvider.GetService<IHubContext<TimerHub>>();
                hub.Clients.All.InvokeAsync("Notify", DateTime.Now);
            };
            //var timer = new Timer(callback);
            //timer.Change(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(10));

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "localizedDefault",
                    template: "{culture:culture}/{controller=Home}/{action=Index}/{id?}",
                    defaults: new { culture = requestLocalizationOptions.Value.DefaultRequestCulture.Culture.Name });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "NotFound",
                    template: "{*url}",
                    defaults: new { controller = "Home", action = "CatchAll" });
            });
        }
    }
}
