using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace WebApplication
{
    public static class ConfigurationExtensions
    {
        public static string RedisConnectionString(this IConfiguration configuration) => 
            configuration["AppSettings:Redis:ConnectionString"];

        public static IEnumerable<string> AllowedCorsOrigins(this IConfiguration configuration)
        {
            var list = new List<string>();
            configuration.GetSection("AppSettings:AllowedCorsOrigins").Bind(list);
            return list;
        }
    }
}