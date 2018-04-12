using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SEP.Movil.Business.Hubs;

namespace SEPSignalR
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder => { builder .AllowAnyMethod() .AllowAnyHeader() .WithOrigins("http://localhost:5000"); })); 
            services.AddSignalR();
            services.AddScoped<MesaDeTicketsHub>();
            services.AddSingleton<TicketManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("CorsPolicy");
            app.UseFileServer();
            app.UseSignalR(routes =>
            {
                routes.MapHub<MesaDeTicketsHub>("MesaDeTicketsHub");
                
            });
        }
    }
}
