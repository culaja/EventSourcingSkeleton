using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApplication.Controllers
{
    public static class HttpContextExtensions
    {
        public static Task<string> ReadRequestBodyAsString(this HttpContext httpContext)
        {
            using var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8);
            return reader.ReadToEndAsync();
        }
    }
}