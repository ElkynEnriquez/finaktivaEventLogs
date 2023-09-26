using EmailMessage.Configuration;
using EmailMessage.Services;
using EmailMessage.Services.Contract;
using FinaktivaEventLogs.API.Extensions;
using FinaktivaEventLogs.API.RabbitHandler;
using FinaktivaEventLogs.Services.AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQBus.EventQueue;
using RabbitMQBus.Implement;
using RabbitMQBus.RabbitBus;
using System.Text;

namespace FinaktivaEventLogs.API
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Agregar autenticación de JWT Bearer
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                        .GetBytes(Configuration.GetSection("TokenKey:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            //Versionado de Api
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            });


            //Inicializar RabbitEventBus Inyección de Dependencias y MediaTR
            services.AddSingleton<IRabbitEventBus, RabbitEventBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitEventBus(sp.GetService<IMediator>(), scopeFactory);
            });
            services.AddMediatR(typeof(Startup));
            services.AddTransient<NotificationEmailEventHandler>();
            services.AddTransient<IRabbitEventBus, RabbitEventBus>();
            services.AddTransient<IEventHandler<NotificationEmailEventQueue>, NotificationEmailEventHandler>();

            //Agregar configuración de correos
            var emailConfig = Configuration
                 .GetSection("EmailSettings")
                 .Get<EmailSettings>();
            services.AddSingleton(emailConfig);

            //Inicializar Servicio de Correos
            services.AddScoped<IEmailService, EmailService>();

            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling =
                Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            #region AuttoMappers
            services.AddAutoMapper(typeof(Startup).Assembly, typeof(EventLogProfile).Assembly);
            #endregion

            services.AddRepositories();
            services.AddServices();
            services.AddDbConfiguration(Configuration.GetConnectionString("FinaktivaEventLogs.DBConnection"));
            services.AddLoggerService();
            services.AddCorsDomainConfiguration(MyAllowSpecificOrigins);
            services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FinaktivaEventLogs", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            //Clientes HTTP (Comunicación entre microservicios)
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FinaktivaEventLogs v1"));
            }

            if (env.IsProduction())
            {
            }

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var eventBus = app.ApplicationServices.GetRequiredService<IRabbitEventBus>();
            eventBus.Subscribe<NotificationEmailEventQueue, NotificationEmailEventHandler>();
        }

    }
}
