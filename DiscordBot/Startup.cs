using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DiscordBot.Services;

namespace DiscordBot
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RPGContext>(options =>
            {
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=RPGContext;Trusted_Connection=True;MultipleActiveResultSets=true");
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IMapService, MapService>();
            services.AddScoped<IProfileService, ProfileService>();

            var serviceProvider = services.BuildServiceProvider(); // i don't know how to fix this warning

            Bot bot = new Bot(serviceProvider);
            services.AddSingleton(bot);

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
