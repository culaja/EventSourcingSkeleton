using System.Threading.Tasks;
using Framework;
using Microsoft.AspNetCore.Http;
using static System.Net.HttpStatusCode;

namespace WebApplication.Middlewares
{
    public sealed class BadRequestExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        
        public BadRequestExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequestException ex)
            {
                context.Response.StatusCode = (int) BadRequest;
                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}