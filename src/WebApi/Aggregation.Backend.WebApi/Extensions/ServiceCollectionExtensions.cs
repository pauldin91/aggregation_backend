using Aggregation.Backend.Domain.Constants;

namespace Aggregation.Backend.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebApiExtensions(this IServiceCollection services)
        {
            services.AddOutputCache(s =>
            {
                s.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(10);
                s.AddPolicy(Policies.AggregatesCachePolicy, builder =>
                {
                    builder.SetVaryByQuery(Policies.KeywordQueryParam, Policies.FilterQueryParam, Policies.SortQueryParam, Policies.SortTypeQueryParam);
                });
            });

            return services;
        }
    }
}