using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models.DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace SportsPro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSession();

            services.AddControllersWithViews();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<SportsProContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SportsPro")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<SportsProContext>()
                .AddDefaultTokenProviders();

            services.AddRouting(options => {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = true;
            });
        }

        // Use this method to configure the HTTP request pipeline.
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

            app.UseAuthentication();
            app.UseAuthorization();

            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                ConfigureIdentity.CreateAdminUserAsync(scope.ServiceProvider).GetAwaiter().GetResult();
            }

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}