using System;
using System.Collections.Generic;
using System.Linq;
using InMemoryAdapter;
using Microsoft.Extensions.Configuration;
// ReSharper disable ClassNeverInstantiated.Local

namespace WebApplication
{
    public static class ConfigurationExtensions
    {
        public static string EventStoreConnectionString(this IConfiguration configuration) =>
            configuration["AppSettings:EventStore:ConnectionString"];
        
        public static string EventStoreName(this IConfiguration configuration) =>
            configuration["AppSettings:EventStore:StoreName"];

        public static IReadOnlyList<AggregateTypeCacheExpiration> AggregateTypeCacheExpirations(this IConfiguration configuration)
        {
            var list = new List<AggregateTypeCacheExpirationConfiguration>();
            configuration.GetSection("AppSettings:AggregateTypeCacheExpirations").Bind(list);
            return list.Select(item => AggregateTypeCacheExpiration.Of(
                    item.AggregateType,
                    TimeSpan.FromMilliseconds(item.ExpirationTimeSpanInMs)))
                .ToList();
        }

        public static IEnumerable<string> AllowedCorsOrigins(this IConfiguration configuration)
        {
            var list = new List<string>();
            configuration.GetSection("AppSettings:AllowedCorsOrigins").Bind(list);
            return list;
        }

        private sealed class AggregateTypeCacheExpirationConfiguration
        {
            public string AggregateType { get; set; }
            public uint ExpirationTimeSpanInMs { get; set; }
        }
    }
}