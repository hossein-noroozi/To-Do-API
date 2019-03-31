using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using ToDoApi.Data;

namespace ToDoApi
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
            // Inject DbContext
            services.AddDbContext<ToDoContext>(opt =>
            opt.UseSqlServer(@"Server=.;Database=ToDoApiDB;Trusted_Connection=True;"),
            ServiceLifetime.Transient);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Adding Swagger Generator
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new Info {
                    Title = "My Api",
                    Version = "v1" ,
                    Description = "a simple Api For Having A To Do List",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Hossein Noroozi" ,
                        Email = "Hossein.noroozi0@Gmail.Com" ,
                        Url = "https://Github.com/CrowFather"
                    },
                    License = new License
                    {
                        Name = "Use Under LICX",
                        Url = "https://www.example.com/License"
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //Adding Swagger and swagger UI To PipeLine
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
