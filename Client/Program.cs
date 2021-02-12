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
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Client.Services.Grpc;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;
using Tomi.Calendar.Mono.Shared.Dtos.Note;
using Tomi.Calendar.Mono.Shared.Dtos.Tag;
using Tomi.Calendar.Proto;
using Tomi.Notification.Blazor.Services;

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

            ConfigureFluxor(builder, currentAssembly);
            ConfigureJsonSerializer(builder);
            ConfigureProtobufRuntimeTypeModels();

            ConfigureHttpClients(builder);
            BuildGrpcWebHandler(builder);

            builder.Services.AddApiAuthorization();
            builder.Services.AddBlazoredModal();
            builder.Services.AddBlazoredLocalStorage();

            BuildNotificationServices(builder);
            BuildCalendarServices(builder);

            var host = builder.Build();

            await host.RunAsync();
        }

        private static void ConfigureHttpClients(WebAssemblyHostBuilder builder)
        {
            builder.Services
                .AddHttpClient("Tomi.Calendar.Mono.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services
                .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("Tomi.Calendar.Mono.ServerAPI"));
        }

        private static void ConfigureFluxor(WebAssemblyHostBuilder builder, System.Reflection.Assembly currentAssembly)
        {
            builder.Services.AddFluxor(config =>
            {
                config.ScanAssemblies(currentAssembly).UseReduxDevTools();
            });

            builder.Services.AddScoped<StateFacade>();
        }

        private static void ConfigureProtobufRuntimeTypeModels()
        {
            RuntimeTypeModel.Default.AddNodaTime();
            RuntimeTypeModel.Default.Add(typeof(CalendarItemDto), false).SetSurrogate(typeof(CalendarItemSurrogate));
            RuntimeTypeModel.Default.Add(typeof(NoteDto), false).SetSurrogate(typeof(NoteSurrogate));
            RuntimeTypeModel.Default.Add(typeof(TagDto), false).SetSurrogate(typeof(TagSurrogate));
        }

        private static void ConfigureJsonSerializer(WebAssemblyHostBuilder builder)
        {
            JsonSerializerOptions options = new JsonSerializerOptions(JsonSerializerDefaults.Web).ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            builder.Services.AddSingleton(options);
        }

        private static void BuildGrpcWebHandler(WebAssemblyHostBuilder builder)
        {
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
        }

        private static void BuildNotificationServices(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddNotificationHub(NotificationHubConfig(builder, builder.HostEnvironment.BaseAddress + "notificationHub"));
        }

        private static void BuildCalendarServices(WebAssemblyHostBuilder builder)
        {
            //builder.Services.AddHttpClient<CalendarHttpService>("CalendarApi", client =>
            //    {
            //        // client.BaseAddress = new Uri("https://localhost:8091");
            //        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            //        client.DefaultRequestHeaders.Add("Accept", "application/json");
            //        client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            //    })
            //    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            //builder.Services
            //    .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
            //    .CreateClient("CalendarApi"));

            builder.Services.AddScoped<CalendarGrpcService>();
            builder.Services.AddScoped<CalendarDataService>();
        }

        private static Action<IServiceProvider, NotificationHubServiceOptions> NotificationHubConfig(WebAssemblyHostBuilder builder, string uri)
        {
            return (sp, options) =>
            {
                IAccessTokenProvider accessTokenProvider = sp.GetRequiredService<IAccessTokenProvider>();
                options.Uri = new Uri(uri);
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
            };
        }
    }
}
