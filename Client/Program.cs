using Blazored.LocalStorage;
using Blazored.Modal;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Client.State;

namespace Tomi.Calendar.Mono.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient<CalendarHttpService>("CalendarApi", client =>
            {
                // client.BaseAddress = new Uri("https://localhost:8091");
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            }); //.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("CalendarApi"));

            builder.Services.AddHttpClient("Tomi.Calendar.Mono.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Tomi.Calendar.Mono.ServerAPI"));

            builder.Services.AddApiAuthorization();


            builder.Services.AddSingleton<CalendarItemState>();

            builder.Services.AddBlazoredModal();
            builder.Services.AddBlazoredLocalStorage();

            var host = builder.Build();

            var calendarService = host.Services.GetRequiredService<CalendarItemState>();
            await calendarService.InitializeCalendarItemsAsync();

            await host.RunAsync();
        }
    }
}
