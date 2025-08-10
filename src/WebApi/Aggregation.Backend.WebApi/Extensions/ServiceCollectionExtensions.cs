using Aggregation.Backend.Domain.Constants;
using Aggregation.Backend.WebApi.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Aggregation.Backend.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebApiExtensions(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Aggregation API",
                    Version = $"v{ApiEndpoints.Version}",
                    Description = "Secure API with JWT Authentication"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
            services.AddOutputCache(s =>
            {
                s.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(10);
                s.AddPolicy(Domain.Constants.Policies.AggregatesCachePolicy, builder =>

                     builder.AddPolicy<AuthenticatedCachePolicy>()
                     .SetVaryByQuery(Domain.Constants.Policies.KeywordQueryParam, Domain.Constants.Policies.FilterQueryParam, Domain.Constants.Policies.SortQueryParam, Domain.Constants.Policies.SortTypeQueryParam)
                 ,true);
            });

            return services;
        }
    }
}