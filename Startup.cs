using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVC_REST_API.Data;
using System;
using Newtonsoft.Json.Serialization;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;

namespace MVC_REST_API
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
            services.AddDbContext<CommanderContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("CommanderConnection")));

            services.AddControllers().AddNewtonsoftJson(s => {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //services.AddScoped<ICommanderRepo, MockCommanderRepo>();
            services.AddScoped<ICommanderRepo, SqlCommanderRepo>();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title="Commander API", 
                    Version="v1",
                    Description="A simple MVC REST API with ASP.NET Core 3.1",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact 
                    { 
                        Name="Claude Chris",
                        Email="christ.tchambila@gmail.com",
                        Url=new Uri("https://cchris.netlify.com"),
                    },
                    License = new OpenApiLicense 
                    { 
                        Name="Use under LICX",
                        Url= new Uri("https://example.com/license"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.

            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Commander API V1");
                c.RoutePrefix = string.Empty;
            });
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
