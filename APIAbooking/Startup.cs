using APIAbooking.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation.AspNetCore;
using APIAbooking.Services;
using APIAbooking.Logic.Client;
using Microsoft.AspNetCore.Mvc.Razor;

namespace APIAbooking
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddFluentValidation(mvcConfiguration => mvcConfiguration.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddTransient<IClientService, ClientServices>();
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
            services.AddDbContext<APIAbookingContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            services.AddMvc()
                .AddViewLocalization(
                LanguageViewLocationExpanderFormat.Suffix,
                opts => { opts.ResourcesPath = "APIAbooking/Resources/ClientResorces"; })
                .AddDataAnnotationsLocalization();

            //    services.AddAuthentication()
            //       .AddGoogle(options =>
            //       {
            //           IConfigurationSection googleAuthNSection =
            //               Configuration.GetSection("Authentication:Google");

            //           options.ClientId = googleAuthNSection["ClientId"];
            //           options.ClientSecret = googleAuthNSection["ClientSecret"];
            //       });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Clients}/{action=Login}/{id?}");
            });
        }

    }
}
