using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Tomi.Calendar.Mono.Server
{
    public static class ConfigureCors
    {
        static readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            string env_origins = Environment.GetEnvironmentVariable("AllowedOrigins") ?? "";
            string env_origins_headers = Environment.GetEnvironmentVariable("AllowedOriginsHeaders") ?? "";
            string env_origins_methods = Environment.GetEnvironmentVariable("AllowedOriginsMethods") ?? "";
            if (env_origins != null)
            {
                string[] origins = env_origins.Split(",");
                string[] originsheaders = env_origins_headers.Split(",");
                string[] originsmethods = env_origins_methods.Split(",");
                serviceCollection.AddCors(options =>
                {
                    options.AddPolicy(name: MyAllowSpecificOrigins, b =>
                    {
                        b.WithOrigins(origins);
                        b.WithHeaders(originsheaders);
                        b.WithMethods(originsmethods);
                    });
                });
            }
        }

        public static void ConfigureApp(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseCors(MyAllowSpecificOrigins);
        }
    }
}
