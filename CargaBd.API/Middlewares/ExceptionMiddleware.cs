using System;
using System.Text.Json;
using System.Threading.Tasks;
using CargaBd.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CargaBd.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next,IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                Log.Error(exception,@"Ha ocurrido un error con mensaje "+exception.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode =  StatusCodes.Status500InternalServerError;
                var response = _env.IsDevelopment()
                    ? new AppException(context.Response.StatusCode, exception.Message, exception.StackTrace)
                    : new AppException(context.Response.StatusCode, "Error en el servidor");
                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
