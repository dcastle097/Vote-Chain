using System;
using System.IO;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using VotingSystem.Domain.DTOs.Requests;
using VotingSystem.Domain.DTOs.Validations;
using VotingSystem.Infrastructure.Models.Configuration;
using VotingSystem.Infrastructure.Repositories;
using VotingSystem.Infrastructure.Repositories.Interfaces;
using VotingSystem.Infrastructure.Services;
using VotingSystem.Infrastructure.Services.Interfaces;

namespace VotingSystem.Web.VoterApi;

/// <summary>ASP.Net Core API configuration definition</summary>
public class Startup
{
    /// <summary>
    ///     Ctor. <see cref="Startup" />
    /// </summary>
    /// <param name="configuration">
    ///     Application configuration values set via Dependency Injection. See
    ///     <see cref="IConfiguration" />
    /// </param>
    /// <param name="environment">
    ///     Web hosting configuration environment information ser via Dependency Injection. See
    ///     <see cref="IWebHostEnvironment" />
    /// </param>
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
    }

    private IConfiguration Configuration { get; }
    private IWebHostEnvironment Environment { get; }

    /// <summary>
    ///     Add services to the container. This method gets called by the runtime.
    /// </summary>
    /// <param name="services">Services descriptors set via Dependency Injection. See <see cref="IServiceCollection" /></param>
    public void ConfigureServices(IServiceCollection services)
    {
        #region Configuration

        services.Configure<AmazonWebServicesConfiguration>(Configuration.GetSection("AmazonWebServices"));
        services.Configure<MailConfiguration>(Configuration.GetSection("Email"));
        services.Configure<EthConfiguration>(Configuration.GetSection("Eth"));

        #endregion

        #region Repositories

        services.AddTransient<IPollsRepository, PollsRepository>();
        services.AddTransient<IVotersRepository, VotersRepository>();

        #endregion

        #region Services

        services.AddTransient<IMailService>(x =>
            ActivatorUtilities.CreateInstance<MailService>(x, Environment.WebRootPath));
        services.AddTransient<IPollsService, PollsService>();
        services.AddTransient<IVotersService, VotersService>();

        #endregion

        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        services.AddControllers().AddFluentValidation();

        #region Validators

        services.AddTransient<IValidator<VoterActivationRequestDto>, VoterSetPasswordRequestValidator>();

        #endregion

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Voter API",
                Version = "v1"
            });
            var filePath = Path.Combine(AppContext.BaseDirectory,
                Assembly.GetExecutingAssembly().GetName().Name + ".xml");
            c.IncludeXmlComments(filePath);
        });
    }

    /// <summary>
    ///     Configure the HTTP request pipeline. This method gets called by the runtime.
    /// </summary>
    /// <param name="app">
    ///     Application pipeline configuration set via Dependency Injection. See
    ///     <see cref="IApplicationBuilder" />
    /// </param>
    public void Configure(IApplicationBuilder app)
    {
        if (Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VotingSystem.Api.Admin v1"));
        }

        if (string.IsNullOrWhiteSpace(Environment.WebRootPath))
            Environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}