using AutoMapper;
using DroneDelivery.Application.Interfaces;
using DroneDelivery.Application.Services;
using DroneDelivery.Data.Data;
using DroneDelivery.Data.Repositorios;
using DroneDelivery.Data.Repositorios.IRepository;
using DroneDelivery.Domain.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace DroneDelivery.Api
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
            services.AddDbContext<DroneDbContext>(opts =>
            {
                opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Drone Delivery API",
                    Description = "Entrega de pedidos via drone",
                    Contact = new OpenApiContact
                    {
                        Name = "Grupo 5",
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opts.IncludeXmlComments(xmlPath);
            });



            services.AddAutoMapper(typeof(DroneService).Assembly);

            services.AddControllers();


            services.AddScoped<IDroneService, DroneService>();
            services.AddScoped<IPedidoService, PedidoService>();

            services.AddScoped<IDroneRepository, DroneRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IIntinerarioRepository, IntinerarioRepository>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Drone Delivery API V1");
                c.RoutePrefix = string.Empty;
            });


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
