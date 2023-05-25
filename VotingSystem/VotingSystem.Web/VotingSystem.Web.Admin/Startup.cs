using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VotingSystem.Domain.DTOs.Requests;
using VotingSystem.Domain.DTOs.Validations;
using VotingSystem.Infrastructure.Models.Configuration;
using VotingSystem.Infrastructure.Repositories;
using VotingSystem.Infrastructure.Repositories.Interfaces;
using VotingSystem.Infrastructure.Services;
using VotingSystem.Infrastructure.Services.Interfaces;
using VotingSystem.Web.Admin.Models.Configuration;
using VotingSystem.Web.Admin.Services;
using VotingSystem.Web.Admin.Services.Interfaces;

namespace VotingSystem.Web.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.IsDevelopment())
            {
                var builder = services.AddRazorPages();
                builder.AddRazorRuntimeCompilation();
            }

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.Always;
            });
            services.Configure<CookieTempDataProviderOptions>(options => { options.Cookie.IsEssential = true; });

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/";
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = context =>
                        {
                            context.Response.Redirect($"/?ReturnUrl={context.Request.Path.ToUriComponent()}");
                            return Task.CompletedTask;
                        }
                    };
                    options.AccessDeniedPath = "/";
                });

            services.AddControllersWithViews().AddSessionStateTempDataProvider().AddFluentValidation();
            services.AddSession(options => { options.Cookie.IsEssential = true; });
            
            #region Configuration
            
            services.Configure<AdminUserConfiguration>(Configuration.GetSection("AdminUser"));
            services.Configure<AmazonWebServicesConfiguration>(Configuration.GetSection("AmazonWebServices"));
            services.Configure<MailConfiguration>(Configuration.GetSection("Email"));
            services.Configure<EthConfiguration>(Configuration.GetSection("Eth"));
            
            #endregion

            #region Repositories

            services.AddTransient<IPollsRepository, PollsRepository>();
            services.AddTransient<IVotersRepository, VotersRepository>();

            #endregion

            #region Services

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IMailService>(x => 
                ActivatorUtilities.CreateInstance<MailService>(x, Environment.WebRootPath));
            services.AddTransient<IPollsService, PollsService>();
            services.AddTransient<IVotersService, VotersService>();

            #endregion

            #region Validators

            services.AddTransient<IValidator<LoginRequestDto>, LoginRequestValidator>();
            services.AddTransient<IValidator<PollRequestDto>, PollRequestValidator>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}