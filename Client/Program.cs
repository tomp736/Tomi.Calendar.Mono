using Blazored.LocalStorage;
using Blazored.Modal;
using Fluxor;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using ProtoBuf.Grpc.Client;
using ProtoBuf.Meta;
using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Tomi.Blazor.Notification.Services;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Client.Store.Features.CalendarItem;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;
using Tomi.Calendar.Proto.CodeFirst;

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


            builder.Services.AddHttpClient<RestDataTableServiceClient>("DataTableApi", client =>
            {
                // client.BaseAddress = new Uri("https://localhost:8091");
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("DataTableApi"));

            builder.Services.AddHttpClient("Tomi.Calendar.Mono.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Tomi.Calendar.Mono.ServerAPI"));

            builder.Services.AddApiAuthorization();

            builder.Services.AddScoped<StateFacade>();

            builder.Services.AddSingleton<NotificationService>();

            builder.Services.AddBlazoredModal();
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddScoped(services =>
            {
                // Get the service address from appsettings.json
                var config = services.GetRequiredService<IConfiguration>();
                var backendUrl = config["BackendUrl"];

                // If no address is set then fallback to the current webpage URL
                if (string.IsNullOrEmpty(backendUrl))
                {
                    var navigationManager = services.GetRequiredService<NavigationManager>();
                    backendUrl = navigationManager.BaseUri;
                }

                var baseAddressMessageHandler = services.GetRequiredService<BaseAddressAuthorizationMessageHandler>();
                baseAddressMessageHandler.InnerHandler = new HttpClientHandler();
                var grpcWebHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, baseAddressMessageHandler);

                return GrpcChannel.ForAddress(backendUrl, new GrpcChannelOptions
                {
                    HttpHandler = grpcWebHandler,
                    MaxReceiveMessageSize = int.MaxValue,
                    MaxSendMessageSize = int.MaxValue
                });
            });

            RuntimeTypeModel.Default.Add(typeof(DataTable), false).SetSurrogate(typeof(DataTableSurrogate));
            RuntimeTypeModel.Default.Add(typeof(CalendarItemDto), false).SetSurrogate(typeof(CalendarItemSurrogate));
            RuntimeTypeModel.Default.AddNodaTime();

            builder.Services.AddSingleton<GrpcHelloService>();
            builder.Services.AddSingleton<GrpcDataTableServiceClient>();
            builder.Services.AddScoped<GrpcCalendarItemServiceClient>();

            var host = builder.Build();

            await host.RunAsync();
        }
    }
}
