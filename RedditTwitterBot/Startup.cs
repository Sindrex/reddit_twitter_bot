using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedditTwitterBot.Features.RedditScrape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedditTwitterBot
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}"
                );
            });

            StartBot();
        }

        public void StartBot()
        {
            int delay = 2000; //ms
            Task task = Task.Run(async () => await BotLoop(delay));
        }

        private async Task BotLoop(int delay)
        {
            bool error = false;
            while (!error)
            {
                try
                {
                    var res = await RedditScrape.Scrape("https://www.reddit.com/top/");
                    Console.WriteLine("Posts at " + DateTime.Now.ToString("F"));
                    foreach(var item in res)
                    {
                        Console.WriteLine(item);
                    }
                    await Task.Delay(delay);
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} \n {1}", e.Message, e.StackTrace);
                    error = true;
                }
            }
            return;
        }
    }
}
