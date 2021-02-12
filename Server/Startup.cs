using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using Npgsql;
using ProtoBuf.Grpc.Server;
using ProtoBuf.Meta;
using System.Data;
using System.Linq;
using Tomi.Calendar.Mono.Server.Data;
using Tomi.Calendar.Mono.Server.DataServices;
using Tomi.Calendar.Mono.Server.Models;
using Tomi.Calendar.Mono.Server.Services.Notification;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;
using Tomi.Calendar.Mono.Shared.Dtos.Note;
using Tomi.Calendar.Mono.Shared.Dtos.Tag;
using Tomi.Calendar.Proto;
using Tomi.Notification.AspNetCore;
using Tomi.Notification.AspNetCore.Hubs;
using Tomi.Notification.AspNetCore.Services;

namespace Tomi.Calendar.Mono.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DbContextEvents>();
            services.AddDbContext<AppNpgSqlDataContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), o => o.UseNodaTime());
            });

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AppNpgSqlDataContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, AppNpgSqlDataContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddControllersWithViews()
                .AddJsonOptions(opt => opt.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb));
            services.AddRazorPages();


            RuntimeTypeModel.Default
                .Add(typeof(CalendarItemDto), false)
                .SetSurrogate(typeof(CalendarItemSurrogate));

            RuntimeTypeModel.Default
                .Add(typeof(NoteDto), false)
                .SetSurrogate(typeof(NoteSurrogate));

            RuntimeTypeModel.Default
                .Add(typeof(TagDto), false)
                .SetSurrogate(typeof(TagSurrogate));

            RuntimeTypeModel.Default
                .AddNodaTime();

            services.AddGrpc(options =>
            {
                options.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
                options.EnableDetailedErrors = true;
                options.MaxReceiveMessageSize = int.MaxValue;
                options.MaxSendMessageSize = int.MaxValue;
            });
            services.AddCodeFirstGrpc(options =>
            {
                options.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
                options.EnableDetailedErrors = true;
                options.MaxReceiveMessageSize = int.MaxValue;
                options.MaxSendMessageSize = int.MaxValue;
            });
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            services.AddSignalR();
            //services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

            services.AddTransient<INotificationProcessingServiceDataProvider, NotificationProcessingServiceDataProvider>();
            services.AddScoped<INotificationProcessingService, NotificationProcessingService>();
            services.AddNotificationHostedService((sp, options) =>
            {
                options = new NotificationHostedServiceOptions(services);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseWebAssemblyDebugging();
                // ConfigureSwagger.ConfigureApp(app);
            }
            else
            {
                app.UseHttpsRedirection();
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.                
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseRouting();

            // ConfigureCors.ConfigureApp(app);

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseGrpcWeb();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GrpcCalendarItemService>()
                    .RequireAuthorization(new AuthorizeAttribute())
                    .EnableGrpcWeb();

                endpoints.MapGrpcService<GrpcNoteService>()
                    .RequireAuthorization(new AuthorizeAttribute())
                    .EnableGrpcWeb();

                endpoints.MapGrpcService<GrpcTagService>()
                    .RequireAuthorization(new AuthorizeAttribute())
                    .EnableGrpcWeb();

                endpoints.MapHub<NotificationHub>("/notificationhub");

                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
