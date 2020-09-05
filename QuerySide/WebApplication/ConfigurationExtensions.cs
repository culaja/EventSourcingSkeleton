﻿using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace WebApplication
{
    public static class ConfigurationExtensions
    {
        public static IEnumerable<string> AllowedCorsOrigins(this IConfiguration configuration)
        {
            var list = new List<string>();
            configuration.GetSection("AppSettings:AllowedCorsOrigins").Bind(list);
            return list;
        }
    }
}