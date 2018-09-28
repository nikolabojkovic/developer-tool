using Email.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using Core;
using Core.Interfaces;
using Infrastructure.DbContexts;
using Infrastructure.Models;
using Infrastructure.Core;
using Domain.Interfaces;
using Domain.Services;
using WebApi.Validation;
using Infrastructure.Data;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using System.Reflection;
using AutoMapper;

namespace TestIntegration
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

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
            services.AddDbContext<BackOfficeContext>(opt => opt.UseInMemoryDatabase("backoffice_database"));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IEmailService, EmailService>();            
            services.AddAutoMapper(typeof(Startup));
            services.AddMvc(opt => {
                opt.Filters.Add(typeof(ValidatorActionFilter));
                opt.OutputFormatters.Add(new HtmlOutputFormatter());
            }).AddFluentValidation(fvc =>
                fvc.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        public void Configure(IApplicationBuilder app)
        {            
            app.UseCors("AllowAllOrigins");
            app.UseMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            Assembly[] assemblies = {
                Assembly.Load("Infrastructure"),
                Assembly.Load("Domain"),
                Assembly.Load("Core"),
                Assembly.Load("WebApi")
            };

            builder.RegisterAssemblyTypes(assemblies)
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces();  
            
            builder.RegisterGeneric(typeof(Repository<>))
                   .As(typeof(IRepository<>));
        }

        public void ConfigureTesting(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            // this.Configure(app, env, loggerFactory);
            this.Configure(app);
            PopulateTestData(app);
        }

        private void PopulateTestData(IApplicationBuilder app)
        {
            var dbContext = app.ApplicationServices.GetService<BackOfficeContext>();
            DbInitializer.SeedEvents(dbContext);
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