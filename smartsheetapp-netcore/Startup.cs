using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using smartsheetapp_netcore.Data;
using smartsheetapp_netcore.Services;

namespace smartsheetapp_netcore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IEventsService, EventsService>();
            services.AddScoped<ICallbackService, CallbackService>();
            services.AddScoped<ISmartsheetService, SmartsheetService>();
            services.AddScoped<IManageWebhooksService, ManageWebhooksService>();
            services.AddHostedService<OngoingService>();
            services.AddControllersWithViews();
            services.AddDbContext<EventsContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("EventsContext")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                app.UseExceptionHandler("/Home/Error");
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
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
