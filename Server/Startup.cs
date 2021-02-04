using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using Npgsql;
using ProtoBuf.Grpc.Server;
using System.Linq;
using Tomi.Calendar.Mono.Server.Data;
using Tomi.Calendar.Mono.Server.Models;

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
                endpoints.MapGrpcService<GrpcDataTableService>().EnableGrpcWeb();
                endpoints.MapGrpcService<GrpcGreeterService>().EnableGrpcWeb();
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
