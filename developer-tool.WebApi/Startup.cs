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
using Swashbuckle.AspNetCore.Swagger;
using Persistence.DbContexts;
using Persistence.Core;
using Core.Interfaces;
using WebApi.Validation;
using Autofac;
using System.Reflection;
using AutoMapper;
using System.Linq;
using MediatR;
using WebApi.Middlewares;
using Core.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

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
                     opt.UseMySql(Configuration.GetConnectionString("MySqlConnection"),
                                  x => x.MigrationsAssembly("Persistence")));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddMediatR(new Assembly[] { Assembly.Load("Application") });
            services.AddDistributedMemoryCache();
            services.AddAutoMapper();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Developer-tool API", Version = "v1" });
 
                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
            
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            services.AddMvc(opt => {
                opt.Filters.Add(typeof(ValidatorActionFilter));
                opt.Filters.Add(new AuthorizeFilter(policy));
                opt.OutputFormatters.Add(new HtmlOutputFormatter());
            }).AddFluentValidation(fvc =>
                fvc.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddOptions();
            services.Configure<CacheOptions>(Configuration.GetSection("Cache:CacheOptions"));
            services.Configure<JwtOptions>(Configuration.GetSection("Jwt"));  
        }

        public void Configure(IApplicationBuilder app,  IHostingEnvironment env, ILoggerFactory loggerFactory)
        {     
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection(); 

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
            app.UseAuthentication();
            app.UseMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            Assembly[] assemblies = {
                Assembly.Load("Persistence"),
                Assembly.Load("Infrastructure"),
                Assembly.Load("Application")
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