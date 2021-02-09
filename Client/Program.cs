using Blazored.LocalStorage;
using Blazored.Modal;
using Fluxor;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using ProtoBuf.Meta;
using System;
using System.Data;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Tomi.Notification.Blazor.Services;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;
using Tomi.Calendar.Proto;

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

            builder.Services
                .AddSingleton(options);

            builder.Services
                .AddHttpClient<CalendarHttpService>("CalendarApi", client =>
                {
                    // client.BaseAddress = new Uri("https://localhost:8091");
                    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
                })
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services
                .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("CalendarApi"));

            builder.Services
                .AddHttpClient("Tomi.Calendar.Mono.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services
                .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("Tomi.Calendar.Mono.ServerAPI"));

            builder.Services
                .AddApiAuthorization();

            builder.Services
                .AddScoped<StateFacade>();


            builder.Services.AddNotificationHub((sp, options) =>
            {
                IAccessTokenProvider accessTokenProvider = sp.GetRequiredService<IAccessTokenProvider>();
                options.Uri = new Uri(builder.HostEnvironment.BaseAddress + "notificationhub");
                options.AccessTokenProvider = async () =>
                {
                    AccessTokenResult accessTokenResult = await accessTokenProvider.RequestAccessToken();
                    AccessToken accessToken;
                    if (accessTokenResult.TryGetToken(out accessToken))
                    {
                        return accessToken.Value;
                    }
                    return "";
                };
            });

            builder.Services
                .AddBlazoredModal();
            builder.Services
                .AddBlazoredLocalStorage();

            builder.Services.
                AddScoped(services =>
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

            RuntimeTypeModel.Default
                .Add(typeof(CalendarItemDto), false)
                .SetSurrogate(typeof(CalendarItemSurrogate));

            RuntimeTypeModel.Default
                .AddNodaTime();

            builder.Services
                .AddScoped<GrpcCalendarItemServiceClient>();

            //RuntimeTypeModel.Default.Add(typeof(DataTable), false).SetSurrogate(typeof(DataTableSurrogate));
            //builder.Services.AddSingleton<GrpcDataTableServiceClient>();

            var host = builder.Build();

            await host.RunAsync();
        }
    }
}
