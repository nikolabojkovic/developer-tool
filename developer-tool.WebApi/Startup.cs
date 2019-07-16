using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using FluentValidation.AspNetCore;
using System;
using System.Threading.Tasks;
using Email.Services;
using Infrastructure.DbContexts;
using Infrastructure.Core;
using Core.Interfaces;
using WebApi.Validation;
using Autofac;
using System.Reflection;
using AutoMapper;
using System.Linq;
using MediatR;
using WebApi.Middlewares;
using Microsoft.OpenApi.Models;
using Core.Options;

namespace WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);
                 // .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => { 
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
                    });
            });
            services.AddDbContext<InMemoryContext>(opt => opt.UseInMemoryDatabase("InMemoryDatabase"));
            services.AddDbContext<BackOfficeContext>(opt =>
                     opt.UseMySQL(Configuration.GetConnectionString("MySqlConnection"),
                                  x => x.MigrationsAssembly("Persistence")));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddMediatR(new Assembly[] { Assembly.Load("Application") });
            services.AddDistributedMemoryCache();
            services.AddAutoMapper();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Developer-tool API", Version = "v1" });
            });
            services.AddMvc(opt => {
                opt.Filters.Add(typeof(ValidatorActionFilter));
                opt.OutputFormatters.Add(new HtmlOutputFormatter());
            }).AddFluentValidation(fvc =>
                fvc.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddOptions();
            services.Configure<CacheOptions>(Configuration.GetSection("Cache:CacheOptions"));  
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {     
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();       
            app.Use(async (HttpContext context, Func<Task> next) =>
            {
                await next.Invoke();

                if (context.Response.StatusCode == 404  && !context.Request.Path.Value.Contains("/api"))
                {
                    context.Request.Path = new PathString("/index.html");
                    await next.Invoke();
                }
            });

            app.UseSwagger();            
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Developer-tool API V1");
                c.RoutePrefix = "api";
            });
            app.UseCors("AllowAllOrigins");
            app.UseMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            Assembly[] assemblies = {
                Assembly.Load("Persistence"),
                Assembly.Load("Infrastructure"),
                Assembly.Load("Application"),
                Assembly.Load("Core")
            };

            builder.RegisterAssemblyTypes(assemblies)
                   .Where(t => t.Name.EndsWith("Service") 
                            || t.Name.EndsWith("Provider"))
                   .AsImplementedInterfaces();  
            
            builder.RegisterGeneric(typeof(Repository<>))
                   .As(typeof(IRepository<>));
        }
    }

    public class HtmlOutputFormatter : StringOutputFormatter
    {
        public HtmlOutputFormatter()
        {
            SupportedMediaTypes.Add("text/html");
        }
    }
}