using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationApi.ErrorHandling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthenticationApi
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
            var domains = Configuration.GetSection("Cors:Domains").GetChildren().Select(c => c.Value).ToArray();
            if (domains?.Any() == true) {
                services.AddCors(options => 
                    options.AddPolicy("default", policy => policy.WithMethods("POST,DELETE").WithOrigins(domains).AllowAnyHeader()));
            }
            services.AddMvc(options => options.Filters.Add<GlobalExceptionFilter>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePages("text/plain", "Resource not found. Please check your url.");
            }
            app.UseMvc();
        }
    }
}
