using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.Models;
using Shop.Models.Domain;
using System.Security.Claims;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Shop.Data;
using Shop.Data.Repositories;
using Shop.Filters;
using Shop.Models.Domain.Interface;
using Shop.Services;

namespace Shop
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Startup>();
            Configuration = builder.Build();
            foreach (var item in configuration.AsEnumerable())
            {
                Configuration[item.Key] = item.Value;
            }
        }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
                o.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = "603856596763638";
                facebookOptions.AppSecret = "d17245e06d771095bfa43ee43ec08820";
                facebookOptions.Fields.Add("first_name");
                facebookOptions.Fields.Add("last_name");
                facebookOptions.Fields.Add("gender");
            });

            //services.AddAuthentication().AddTwitter(twitterOptions =>
            //{
            //    twitterOptions.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
            //    twitterOptions.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
            //    twitterOptions.RetrieveUserDetails = true;
            //});

 
            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = "368540063821-q77iotv06t5uuuree8mkcs43fj8ejpol.apps.googleusercontent.com";
                googleOptions.ClientSecret = "rlwJJV0NgvusXiBhgrtWzlal";
            });

            services.AddScoped<CartSessionFilter>();
            services.AddScoped<ThreeBrosDataInitializer>();
            services.AddSession();
            services.AddScoped<IItemsRepository, ItemsRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISellerRepository, SellerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();

            services.Configure<AuthMessageSenderOptions>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ThreeBrosDataInitializer datainit)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/PageNotFound");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePagesWithReExecute("/Error/{0}");


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");

                routes.MapRoute(
                    name: "search",
                    template: "{controller=Home}/{action=Search}/{SearchType}/{SearchKey?}/{Category?}/{MaxStartPrice?}");

                routes.MapRoute(
                    name: "detail",
                    template: "{controller=Home}/{action=Detail}/{Id?}");

                routes.MapRoute(
                    name: "items",
                    template: "{controller=Home}/{action=Detail}/{Id?}");

                routes.MapRoute(
                    name: "sellerRequestEvaluation",
                    template: "{area:exists}/{controller=Admin}/{action=SellerRequestEvaluation}/{Id?}");

                routes.MapRoute(
                    name: "itemsRequestEvaluation",
                    template: "{area:exists}/{controller=Admin}/{action=ItemsRequest}/{Id?}");

                routes.MapRoute(
                    name: "sellerEdit",
                    template: "{area:exists}/{controller=Admin}/{action=SellerEdit}/{Id?}");

                routes.MapRoute(
                    name: "itemsEdit",
                    template: "{area:exists}/{controller=Admin}/{action=ItemsEdit}/{Id?}");

                routes.MapRoute(
                    name: "soldItemsView",
                    template: "{area:exists}/{controller=Admin}/{action=SoldItemsView}/{Id?}");

                routes.MapRoute(
                    name: "shoppingCart",
                    template: "{controller=ShoppingCart}/{action=Add}/{Id}/{Price}/{Count}");

                routes.MapRoute(
                    name: "createItems",
                    template: "{controller=Manage}/{action=CreateItems}/{Id}");

                routes.MapRoute(
                    name: "Account",
                    template: "{controller=Account}/{action=CheckoutMethode}/{checkoutId}/{returnUrl}");

                routes.MapRoute(
                    name: "createCheckoutitems",
                    template: "{controller=Checkout}/{action=CreateItems}/{index}");

                routes.MapRoute(
                   name: "Payment",
                   template: "{controller=Checkout}/{action=Payment}/{Id}");

                routes.MapRoute(
                   name: "MakeItemsUsable",
                   template: "{controller=Checkout}/{action=MakeItemsUsable}/{Id}");

                routes.MapRoute(
                    name: "Error",
                    template: "{controller=Error}/{action=PageNotFound}/{Id?}");

            }); 
            datainit.InitializeData().Wait();
        }
    }
}
