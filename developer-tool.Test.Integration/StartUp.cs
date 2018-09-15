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
using Infrastructure;
using Infrastructure.Models;
using Domain.Interfaces;
using Domain.Services;
using WebApi.Validation;


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
            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            services.AddDbContext<TestContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            services.AddTransient<ITestService, TestService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IRepository<Test>, Repository<Test>>();
            services.AddTransient<IEmailService, EmailService>();
            
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
            var dbContext = app.ApplicationServices.GetService<TestContext>();
            var items = dbContext.TestItems;
            foreach (var item in items)
            {
                dbContext.Remove(item);
            }
            dbContext.SaveChanges();
            dbContext.TestItems.Add(new Test()
            {
                Id=1,
                FirstName = "Steve Smith"
            });
            dbContext.TestItems.Add(new Test()
            {
                Id=2,
                FirstName = "Neil Gaiman",
            });
            dbContext.SaveChanges();
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