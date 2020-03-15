using System;
using System.Net;
using System.Threading.Tasks;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
               await _next(context);
            }
            catch(Exception exception)
            {
                var statusCode = HttpStatusCode.InternalServerError;

                if (exception is ApiException)
                    statusCode = (exception as ApiException).StatusCode;

                context.Response.StatusCode = (int)statusCode;
                var jsonResponse = new { Message = exception.Message };
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(jsonResponse));
            }
        }
    }
}