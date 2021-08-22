using Integration.Server.Extensions;
using Integration.Server.Models;
using Integration.Server.Providers;
using Integration.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Integration.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSwaggerGen(options => options.AddSwaggerOptions());

            services.AddDbContext<IntegrationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DbConnection")));

            services.AddIdentityCore<IntegrationUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<IntegrationDbContext>();

            services.Configure<AdminOptions>(Configuration.GetSection(nameof(AdminOptions)));
            services.Configure<SecurityOptions>(Configuration.GetSection(nameof(SecurityOptions)));

            services
                .AddAuthentication(authOptions =>
                {
                    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters =
                        new TokenProvider(Configuration).GetTokenValidationParameters();
                });

            services.AddHttpContextAccessor();
            services.AddTransient<IToDoService, ToDoService>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<ITokenProvider, TokenProvider>(provider =>
                new TokenProvider(provider.GetRequiredService<IOptions<SecurityOptions>>()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IntegrationDbContext integrationDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();

                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlazorFocused.Integration v1"));
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            if (integrationDbContext.Database.GetPendingMigrations().Count() > 0)
            {
                integrationDbContext.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
