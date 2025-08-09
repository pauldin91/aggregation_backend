namespace Aggregation.Backend.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebApiExtensions(this IServiceCollection services)
        {
            services.AddOutputCache(s =>
            {
                s.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(10);
                s.AddPolicy("AggregatePolicy", builder =>
                {
                    builder.SetVaryByQuery("category", "filter", "orderBy", "asc");
                });
            });

            return services;
        }
    }
}