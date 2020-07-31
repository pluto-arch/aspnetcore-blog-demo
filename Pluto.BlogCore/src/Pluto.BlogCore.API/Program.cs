using Autofac.Extensions.DependencyInjection;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;

using System;
using System.IO;
using System.Net;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pluto.BlogCore.Infrastructure;

namespace Pluto.BlogCore.API
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            Log.Logger = CreateSerilogLogger(configuration);
            try
            {
                Log.Information("׼������{ApplicationContext}...", AppName);
                var host = BuildWebHost(configuration, args);
                Log.Information("{ApplicationContext} ������", AppName);
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "{ApplicationContext} ���ִ���:{messsage} !", AppName,ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args)
        {
            var webHost = WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(false)
                .UseIISIntegration()
                .ConfigureKestrel(options =>
                {
                    var hostAddress = GetDefinedPorts(configuration);
                    options.Listen(hostAddress.IP, hostAddress.Port, listenOptions =>
                    {
                        listenOptions.Protocols = Enum.TryParse<HttpProtocols>(hostAddress.Protocols, ignoreCase: true,out var protocols) 
                                                      ? SetHostProtocols(protocols) : HttpProtocols.Http1AndHttp2;
                    });
                })
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(configuration)
                .UseSerilog()
                .Build();

            webHost.MigrateDbContext<PlutoBlogCoreDbContext>((context, services) =>
            {
                var logger = services.GetService<ILogger<PlutoBlogCoreDbContext>>();
            });

            return webHost;
        }

        private static HttpProtocols SetHostProtocols(HttpProtocols protocols)
        {
            switch (protocols)
            {
                case HttpProtocols.Http1:
                    return HttpProtocols.Http1;
                case HttpProtocols.Http2:
                    return HttpProtocols.Http2;
                case HttpProtocols.Http1AndHttp2:
                    return HttpProtocols.Http1AndHttp2;
                case HttpProtocols.None:
                    return HttpProtocols.None;
                default:
                    throw new ApplicationException("Protocols ���Ͳ���ȷ�����ܵ�ֵ��Http1��Http2��None��Http1AndHttp2 ");
            }
        }

        private static (IPAddress IP,int Port,string Protocols) GetDefinedPorts(IConfiguration configuration)
        {
            var ip = configuration["Host:IP"];
            var port = configuration.GetValue<int>("Host:Port");
            var protocols = configuration["Host:Protocols"];
            return (IPAddress.Parse(ip), port,protocols);
        }


        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            const string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("logs", @"log.log"), rollingInterval: RollingInterval.Day,
                    outputTemplate: outputTemplate)
                .CreateLogger();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            return builder.Build();

        }
    }



    public static class WebHostExtension
    {
        public static void MigrateDbContext<TContext>(this IWebHost webHost,
            Action<TContext, IServiceProvider> seeder)
            where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("��ʼǨ�����ݿ� {DbContextName}", typeof(TContext).Name);
                    // ����δ�ύ�����ݿ��Ǩ��
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        // ����Ǩ��
                        context.Database.Migrate();
                        logger.LogInformation("��Ǩ�����ݿ� {DbContextName}", typeof(TContext).Name);
                    }
                    seeder?.Invoke(context, webHost.Services); // �������ݳ�ʼ��
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Ǩ�����ݿ�ʱ���� {DbContextName}", typeof(TContext).Name);
                }

            }
        }
    }

}
