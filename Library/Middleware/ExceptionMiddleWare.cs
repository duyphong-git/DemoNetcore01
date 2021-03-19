using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Library.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Api.Library.Middleware
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly ILogger _logger;

        public ExceptionMiddleWare(RequestDelegate next, IHostEnvironment env, ILogger logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment() ?
                    new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
