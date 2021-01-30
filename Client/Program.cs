using Blazored.LocalStorage;
using Blazored.Modal;
using Fluxor;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Services;

namespace Tomi.Calendar.Mono.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            // Add Fluxor
            var currentAssembly = typeof(Program).Assembly;
            builder.Services.AddFluxor(config =>
            {
                config
                    .ScanAssemblies(currentAssembly)
                    .UseReduxDevTools();
            });

            JsonSerializerOptions options = 
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
                .ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

            builder.Services.AddSingleton(options);

            builder.Services.AddHttpClient<CalendarHttpService>("CalendarApi", client =>
            {
                // client.BaseAddress = new Uri("https://localhost:8091");
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("CalendarApi"));

            builder.Services.AddHttpClient("Tomi.Calendar.Mono.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Tomi.Calendar.Mono.ServerAPI"));

            builder.Services.AddApiAuthorization();

            builder.Services.AddScoped<StateFacade>();


            builder.Services.AddBlazoredModal();
            builder.Services.AddBlazoredLocalStorage();

            var host = builder.Build();

            await host.RunAsync();
        }
    }
}
