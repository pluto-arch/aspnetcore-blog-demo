using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pluto.BlogCore.API.Middlewares;
using Pluto.BlogCore.API.Modules;
using Pluto.BlogCore.Infrastructure;
using Pluto.BlogCore.Infrastructure.Providers;
using PlutoData;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pluto.BlogCore.API.Filters;
using Pluto.BlogCore.API.HealthChecks;


namespace Pluto.BlogCore.API
{
	public class Startup
	{
		private const string DefaultCorsName = "default";

		private readonly string conntctionString = string.Empty;

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			conntctionString = configuration.GetConnectionString("PlutoBlogCore.MSSQL");
		}

		public IConfiguration Configuration { get; }

		public ILifetimeScope AutofacContainer { get; private set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			#region api controller

			services.AddControllers(options => { options.Filters.Add<ModelValidateFilter>(); })
			        .AddNewtonsoftJson(options =>
			        {
				        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
				        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
				        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			        });

			services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

			#endregion

			#region HealthChecks

			services.Configure<MemoryCheckOptions>(options =>
			{
				options.Threshold =
					Configuration
						.GetValue<long>("Options:MemoryChkOpt:Threshold");
			});
			services.AddHealthChecks()
			        .AddCheck<DatabaseHealthCheck>("database_check", failureStatus: HealthStatus.Unhealthy,
			                                       tags: new string[] {"database", "sqlServer"})
			        .AddCheck<MemoryHealthCheck>("memory_check", failureStatus: HealthStatus.Degraded);

			#endregion

			#region EventIdProvider

			services.AddScoped(typeof(EventIdProvider));

			#endregion

			#region efcore  ����ʵ�����ʹ�����ݿ�

			services.AddUnitOfWorkDbContext<PlutoBlogCoreDbContext>(DbContextCreateFactory.OptionsAction(conntctionString),
			                                                 ServiceLifetime.Scoped)
			        .AddRepository();

			#endregion

			#region swagger

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo {Title = "Pluto.BlogCore.API", Version = "v1"});

				c.AddSecurityDefinition("Bearer", //Name the security scheme
				                        new OpenApiSecurityScheme
				                        {
					                        Description = "JWT Authorization header using the Bearer scheme.",
					                        Type = SecuritySchemeType
						                        .Http, //We set the scheme type to http since we're using bearer authentication
					                        Scheme =
						                        "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
				                        });

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Id = "Bearer", //The name of the previously defined security scheme.
								Type = ReferenceType.SecurityScheme
							}
						},
						new List<string>()
					}
				});


				// Set the comments path for the Swagger JSON and UI.
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});

			#endregion

			#region cors

			services.AddCors(options =>
			{
				options.AddPolicy(DefaultCorsName,
				                  builder =>
				                  {
					                  builder.AllowAnyOrigin();
					                  builder.AllowAnyHeader();
					                  builder.AllowAnyMethod();
					                  builder.AllowAnyHeader();
				                  });
			});

			#endregion

			#region httpcontext accessor

			services.AddHttpContextAccessor();

			#endregion

			#region automapper

			services.AddAutoMapper(Assembly.GetEntryAssembly());

			#endregion
		}


		/// <summary>
		/// ���õ�����(autofac)����
		/// </summary>
		/// <param name="builder"></param>
		public void ConfigureContainer(ContainerBuilder builder)
		{
			#region MediatoR

			builder.RegisterModule(new MediatorModule());

			#endregion


			#region Application

			builder.RegisterModule(new ApplicationModule());

			#endregion
		}


		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
			if (env.IsProduction())
			{
				app.UseHsts();
				app.UseHttpsRedirection();
			}

			app.UseExceptionProcess();
			app.UseStaticFiles();
			app.UseSwagger();
			app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pluto.BlogCore.API"); });
			app.UseCors(DefaultCorsName);
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapHealthChecks("/health", new HealthCheckOptions
				{
					ResponseWriter = async (c, r) =>
					{
						c.Response.ContentType = "application/json";
						var result = JsonConvert.SerializeObject(r.Entries);
						await c.Response.WriteAsync(result);
					}
				});
				endpoints.MapControllers();
			});
		}
	}


	/// <summary>
	/// ָ�����ʱdbcontext ����
	/// code first Ǩ��ʱʹ��
	/// </summary>
	/// ��program��û��Ĭ�ϵģ�
	/// public static IHostBuilder CreateHostBuilder(string[] args) =>
	/// Host.CreateDefaultBuilder(args)
	/// .ConfigureWebHostDefaults(webBuilder =>
	/// {
	/// });
	/// ʱ������ָ����γ�ʼ������dbcontext
	public class DbContextCreateFactory : IDesignTimeDbContextFactory<PlutoBlogCoreDbContext>
	{
		public PlutoBlogCoreDbContext CreateDbContext(string[] args)
		{
			var configbuild = new ConfigurationBuilder();
			configbuild.AddJsonFile("appsettings.json", optional: true);
			var config = configbuild.Build();
			string conn = config.GetConnectionString("PlutoBlogCore.MSSQL");

			var optionsBuilder = new DbContextOptionsBuilder<PlutoBlogCoreDbContext>();
			OptionsAction(conn).Invoke(optionsBuilder);
			return new PlutoBlogCoreDbContext(optionsBuilder.Options);
		}


		public static Action<DbContextOptionsBuilder> OptionsAction(string sqlConnStr)
		{
			return options =>
			{
				options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddFilter((category, level) =>
					                                                                           category ==
					                                                                           DbLoggerCategory
						                                                                           .Database.Command
						                                                                           .Name
					                                                                           && level == LogLevel
						                                                                           .Information)
				                                                                .AddSerilog()));
				options.UseSqlServer(sqlConnStr,
				                     sqlServerOptionsAction: sqlOptions =>
				                     {
					                     sqlOptions.MigrationsAssembly(typeof(Startup)
					                                                   .GetTypeInfo().Assembly.GetName().Name);
					                     sqlOptions.EnableRetryOnFailure(maxRetryCount: 5,
					                                                     maxRetryDelay: TimeSpan.FromSeconds(30),
					                                                     errorNumbersToAdd: null);
				                     });
			};
		}
	}
}