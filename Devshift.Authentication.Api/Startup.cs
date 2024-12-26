using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Devshift.Authentication.Api.Models;
using Devshift.Authentication.Api.Shared.Facades;
using Devshift.Authentication.Api.Shared.Repositories;
using Devshift.Authentication.Api.Shared.Services;
using Devshift.DateTimeProvider;
using Devshift.Security.Encryption;
using Devshift.VaultService.Models;
using DevShift.defaultexception;
using Flurl.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NLog;

namespace Devshift.Authentication.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services for controllers
            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });

            // Adds health check service
            services.AddHealthChecks();
            var appSettings = new AppSettings
            {
                VaultHost = Configuration.GetSection("VaultHost").Get<string>(),
                VaultToken = Configuration.GetSection("VaultToken").Get<string>(),
            };

            #region Swagger
            // Read Swagger settings
            var swagger = this.Configuration.GetSection("Swagger").Get<SwaggerSettings>();

            // Add Swagger generator
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                    swagger.Version,
                    new OpenApiInfo
                    {
                        Title = swagger.Title,
                        Version = swagger.Version,
                        Contact = new OpenApiContact()
                        {
                            Name = swagger.Name,
                            Email = swagger.Email
                        }
                    }
                );
            });
            #endregion

            #region Dependency Injection
            var securityEncrpytion = new SecurityEncryption();


            var vaultHost = securityEncrpytion.RSADecrypt(appSettings.VaultHost);
            var vaultToken = securityEncrpytion.RSADecrypt(appSettings.VaultToken);


            var vaultService = new VaultService.VaultService(new VaultConfig
            {
                Url = new FlurlClient(vaultHost),
                Token = vaultToken
            });

            var credentialTxt = vaultService.GetCredential("secret/data/member").Result;
            var ocelotSecretToken = vaultService.GetCredential("secret/data/shared/ocelot").Result;

            var credential = JsonConvert.DeserializeObject<Credential>(credentialTxt);

            var dateTimeProvide = new DateTimeProvider.DateTimeProvider(DateTime.Now);
            services.AddTransient<IDateTimeProvider, DateTimeProvider.DateTimeProvider>(x => dateTimeProvide);

            var nappRepository = new MemberRepository(credential.MemberConnectionString, credential.SslMode, credential.Certificate);
            services.AddTransient<IMemberRepository, MemberRepository>(x => nappRepository);

            var jwtService = new JwtService(ocelotSecretToken);
            services.AddTransient<IJwtService, JwtService>(x => jwtService);

            services.AddTransient<IAuthenFacade, AuthenFacade>();
            services.AddTransient<IUsersFacade, UsersFacade>();
            services.AddTransient<IDefaultLoginFacade, DefaultLoginFacade>();

            var securityEncryption = new SecurityEncryption(credential.HashKey);
            services.AddSingleton<ISecurityEncryption, SecurityEncryption>(x => securityEncryption);

            var SecurityService = new SecurityService(securityEncryption);
            services.AddSingleton<ISecurityService, SecurityService>(x => SecurityService);


            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                var swaggerSettings = this.Configuration.GetSection("Swagger").Get<SwaggerSettings>();
                var serviceName = this.Configuration.GetValue<string>("Consul:Discovery:ServiceName");

                // Use Swagger middleware
                app.UseSwagger(c =>
                {
                    c.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        var forwardedHost = httpReq.Headers["X-Forwarded-Host"].FirstOrDefault();

                        if (string.IsNullOrEmpty(forwardedHost))
                            swagger.Servers = new List<OpenApiServer>
                            {
                                new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" }
                            };
                        else
                            swagger.Servers = new List<OpenApiServer>
                            {
                                new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Headers["X-Forwarded-Host"].FirstOrDefault()}/{serviceName}/" }
                            };
                    });
                });

                // Use Swagger UI middleware
                app.UseSwaggerUI(
                    c =>
                    {
                        c.SwaggerEndpoint($"{swaggerSettings.Endpoint}", swaggerSettings.Title);
                    }
                );
                app.UseDefaultException(_logger, true);
            }
            else
            {
                app.UseDefaultException(_logger, false);
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}