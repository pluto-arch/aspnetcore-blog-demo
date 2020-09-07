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
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Pluto.BlogCore.API.Filters;
using Pluto.BlogCore.API.HealthChecks;
using Pluto.BlogCore.Application.HttpServices;
using Pluto.BlogCore.Application.HttpServices.Handlers;
using Pluto.BlogCore.Application.Options;
using Pluto.BlogCore.Domain.DomainModels.Blog;
using PlutoData.Interface;
using Swashbuckle.AspNetCore.SwaggerGen;
using ILogger = Serilog.ILogger;


namespace Pluto.BlogCore.API
{
    public class Startup
    {
        private const string DefaultCorsName = "default";

        private readonly string conntctionString;

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

            services.AddControllers(options =>
            {
                options.Filters.Add<ModelValidateFilter>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            #endregion

            #region HealthChecks

            services.Configure<MemoryCheckOptions>(options =>
            {
                options.Threshold = Configuration.GetValue<long>("Options:MemoryChkOpt:Threshold");
            });
            services.AddHealthChecks()
                    .AddCheck<DatabaseHealthCheck>("database_check", failureStatus: HealthStatus.Unhealthy,
                                                   tags: new string[] {"database", "sqlServer"})
                    .AddCheck<MemoryHealthCheck>("memory_check", failureStatus: HealthStatus.Degraded);

            #endregion

            #region EventIdProvider

            services.AddScoped(typeof(EventIdProvider));

            #endregion

            #region efcore  根据实际情况使用数据库

            services
                .AddUnitOfWorkDbContext<PlutoBlogCoreDbContext>(DbContextCreateFactory.OptionsAction(conntctionString),
                                                                ServiceLifetime.Scoped).AddRepository();

            #endregion


            #region identity server

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var identityUrl = Configuration.GetValue<string>("IdentityServer:IdentityUrl");
            //认证
            services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                            .AddIdentityServerAuthentication(x =>
                    {
                        x.Authority = identityUrl; //鉴权服务地址
                        x.RequireHttpsMetadata = false;
                        x.ApiName = "BlogCoreApi";
                        x.SaveToken = true;
                        x.JwtBearerEvents=new JwtBearerEvents
                        {
                            OnTokenValidated= OnTokenValidated
                        };
                    });
                    // .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                    // {
                    //     options.Authority = identityUrl;
                    //     options.RequireHttpsMetadata = false;
                    //     options.SaveToken = true;
                    //     options.TokenValidationParameters = new TokenValidationParameters
                    //     {
                    //         RoleClaimType = JwtClaimTypes.Role,
                    //         NameClaimType = JwtClaimTypes.Name,
                    //         ValidateAudience = false
                    //     };
                    //     options.Events = new JwtBearerEvents
                    //     {
                    //         //OnTokenValidated = OnTokenValidated
                    //         OnTokenValidated = async context =>
                    //         {
                    //             // var token = await context.HttpContext.GetTokenAsync("id_token");
                    //             // Console.WriteLine($"token is {token}");
                    //         }
                    //     };
                    // });

            // 授权
            services.AddAuthorization(o =>
            {
                // 添加授权策略
            });

            #endregion

            #region swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Pluto.BlogCore.API", Version = "v1"});
                c.DescribeAllEnumsAsStrings();
                c.AddSecurityDefinition("oauth2", //Name the security scheme
                                        new OpenApiSecurityScheme
                                        {
                                            Description = "OAuth2.",
                                            Type = SecuritySchemeType.OAuth2,
                                            Flows = new OpenApiOAuthFlows
                                            {
                                                //http://localhost:5000/connect/authorize
                                                //?response_type=token
                                                //&client_id=vue-client
                                                //&redirect_uri=http%3A%2F%2Flocalhost%3A5009%2Fswagger%2Foauth2-redirect.html
                                                //&scope=BlogCoreApi&state=U2F0IEF1ZyAyOSAyMDIwIDExOjI2OjUyIEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp
                                                Implicit = new OpenApiOAuthFlow
                                                {
                                                    AuthorizationUrl = new Uri($"{identityUrl}/connect/authorize"),
                                                    TokenUrl = new Uri($"{identityUrl}/connect/token"),
                                                    Scopes = new Dictionary<string, string>()
                                                    {
                                                        {"all_access", "博客API"}
                                                    }
                                                }
                                            }
                                        });
                c.OperationFilter<AuthorizeCheckOperationFilter>();
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            #endregion

            #region cors

            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsName, builder =>
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

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<Pluto.BlogCore.API.AutoMapperProfile>();
                cfg.AddProfile<Pluto.BlogCore.Application.AutoMapperProfile>();
            }, Assembly.GetExecutingAssembly());

            #endregion

            services.AddHttpClient<YuQueAppService>();

            services.Configure<YuqueOption>(Configuration.GetSection(YuqueOption.Yuque));
        }

        private async Task OnTokenValidated(TokenValidatedContext context)
        {
            await Task.Delay(1);
            var aaa = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork<PlutoBlogCoreDbContext>>();
            var d = aaa.GetBaseRepository<Category>();
            var aaad=new ClaimsIdentity();
            aaad.AddClaim(new Claim("jessy","asdas"));
            context.Principal.AddIdentity(aaad);
            context.Success();
        }


        /// <summary>
        /// 配置第三方(autofac)容器
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
            IdentityModelEventSource.ShowPII = true;
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            if (env.IsProduction())
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseExceptionProcess();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pluto.BlogCore.API");
                c.OAuthClientId("swagger");
                c.OAuthAppName("BlogAPISwaggerUi");
            });
            app.UseCors(DefaultCorsName);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

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
    /// 指定设计时dbcontext 工厂
    /// code first 迁移时使用
    /// </summary>
    /// 当program中没有默认的：
    /// public static IHostBuilder CreateHostBuilder(string[] args) =>
    /// Host.CreateDefaultBuilder(args)
    /// .ConfigureWebHostDefaults(webBuilder =>
    /// {
    /// });
    /// 时，必须指定如何初始化创建dbcontext
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
                options.UseLoggerFactory(LoggerFactory.Create(builder => builder
                                                                         .AddFilter((category, level)
                                                                                        => category == DbLoggerCategory
                                                                                                       .Database.Command
                                                                                                       .Name && level
                                                                                           == LogLevel.Information)
                                                                         .AddSerilog()));
                options.EnableSensitiveDataLogging(true);
                options.UseSqlServer(sqlConnStr, sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30),
                                                    errorNumbersToAdd: null);
                });
            };
        }
    }


    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check for authorize attribute
            var hasAuthorize =
                context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (!hasAuthorize) return;

            operation.Responses.TryAdd("401", new OpenApiResponse {Description = "Unauthorized"});
            operation.Responses.TryAdd("403", new OpenApiResponse {Description = "Forbidden"});

            var oAuthScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [oAuthScheme] = new[] {"all_access"}
                }
            };
        }
    }
}