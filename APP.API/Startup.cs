using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.API.Utilities;
using APP.Repository.EFRepo;
using APP.Repository.EFRepo.EntitiesExt;
using APP.Services.Email.Extension;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace APP.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly string CorsPolicy = "APPCORS";
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Core Policy
            services.AddCors(config =>
            {
                config.AddPolicy(CorsPolicy, option =>
                {
                    option.WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins();
                });
            });

            // Add OAuth JWT
            services.AddAuthentication(config =>
            {
                // Check the cookie to confirm that user is authenticated
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                // On sign in, deal out a cookie
                config.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;

                // Check if user is allowed to perform an action
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer("OAuth", config =>
                {
                    // Encrypt JWT Key
                    var secretBytes = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);
                    var key = new SymmetricSecurityKey(secretBytes);

                    // pass token through header
                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = key,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        ClockSkew = TimeSpan.Zero
                    };

                    //// pass token through URI
                    //config.Events = new JwtBearerEvents()
                    //{
                    //    OnMessageReceived = context =>
                    //    {
                    //        if (context.Request.Query.ContainsKey("access_token"))
                    //        {
                    //            context.Token = context.Request.Query["access_token"];
                    //        }
                    //        return Task.CompletedTask;
                    //    }
                    //};
                });

            //services.AddAuthorization(config =>
            //{
            //    var defaultAuthBuilder = new AuthorizationPolicyBuilder();
            //    var defaultAuthPolicy = defaultAuthBuilder
            //    .RequireAuthenticatedUser()
            //    .RequireClaim(ClaimTypes.Sid)
            //    .Build();

            //    config.DefaultPolicy = defaultAuthPolicy;
            //});

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<AppDBContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Postgress"));
            });

            services.AddIdentity<IUser, IRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders();

            services.AddSwaggerDocumentation();

            services.AddVersionedApiExplorer(options =>
            {
                options.SubstituteApiVersionInUrl = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new MediaTypeApiVersionReader();
                config.ApiVersionSelector = new CurrentImplementationApiVersionSelector(config);
            });

            services.AddMvc(c => c.Conventions.Add(new ApiExplorerGroupPerVersionConvention()));

            services.AddSingleton(Configuration.GetSection("EmailConfig").Get<EmailConfiguration>());

            services.AddControllers();
            services.AddDependencies();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(CorsPolicy);
            app.UseApiVersioning();
            app.UseSwaggerDocumentation();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                .RequireCors(CorsPolicy);
            });
        }
    }
}
