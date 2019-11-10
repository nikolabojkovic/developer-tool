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
using Persistence.DbContexts;
using Persistence.Core;
using WebApi.Validation;
using FluentValidation.AspNetCore;
using Persistence.Data;
using Core.Options;
using MediatR;
using WebApi.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace TestIntegration
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
             new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                                       .Build();
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
            services.AddMediatR(new Assembly[] { Assembly.Load("Application") });                    
            services.AddAutoMapper(typeof(WebApi.Startup));
            services.AddDistributedMemoryCache();
            services.AddOptions();
            services.Configure<CacheOptions>(options =>
            {
                options.IsCachingEnabled = false;
                options.DefaultCacheTime = 0;
            });
            services.Configure<JwtOptions>(options =>  
            {
                options.Key = "JwtTestKey-woifj2f2iefjo2eifj2oef";
                options.Issuer = "http://localhost:5000";
            });
            services.AddMvc(opt => {
                         opt.Filters.Add(typeof(ValidatorActionFilter));
                         opt.OutputFormatters.Add(new HtmlOutputFormatter());
                     })
                     .AddFluentValidation(fvc =>
                         fvc.RegisterValidatorsFromAssemblyContaining<WebApi.Startup>())
                     .AddApplicationPart(Assembly.Load("WebApi"));
        }

        public void Configure(IApplicationBuilder app)
        {             
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseCors("AllowAllOrigins");
            app.UseMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            Assembly[] assemblies = {
                Assembly.Load("Infrastructure"),
                Assembly.Load("Persistence"),
                Assembly.Load("Application")
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
            this.Configure(app);
            PopulateTestData(app);
        }

        private void PopulateTestData(IApplicationBuilder app)
        {
            var dbContext = app.ApplicationServices.GetService<BackOfficeContext>();
            DbInitializer.SeedEvents(dbContext);
            DbInitializer.SeedUsers(dbContext);
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