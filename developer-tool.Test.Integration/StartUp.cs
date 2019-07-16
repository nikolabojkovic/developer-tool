using Email.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Core.Interfaces;
using Autofac;
using System.Reflection;
using AutoMapper;
using Infrastructure.DbContexts;
using Infrastructure.Core;
using WebApi.Validation;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Core.Options;
using Microsoft.Extensions.Options;

namespace TestIntegration
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(env.ContentRootPath)
                 .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true);

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
            services.AddAutoMapper(typeof(WebApi.Startup));
            services.AddDistributedMemoryCache();
            services.AddMvc(opt => {
                opt.Filters.Add(typeof(ValidatorActionFilter));
                opt.OutputFormatters.Add(new HtmlOutputFormatter());
            }).AddFluentValidation(fvc =>
                fvc.RegisterValidatorsFromAssemblyContaining<WebApi.Startup>());
            services.AddOptions();
            services.AddOptions();
            services.Configure<CacheOptions>(Configuration.GetSection("Cache:CacheOptions"));  
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
                Assembly.Load("Application"),
                Assembly.Load("Core"),
                Assembly.Load("Persistence")
            };

            builder.RegisterAssemblyTypes(assemblies)
                   .Where(t => t.Name.EndsWith("Service")                   
                            || t.Name.EndsWith("Provider"))
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