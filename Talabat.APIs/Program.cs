using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using Talabat.APIs.Extentions;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Service;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            #region ConfigureServices
            builder.Services.AddControllers();

            builder.Services.AddSwaggerServices();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(s =>
            {
                var Connection = builder.Configuration.GetConnectionString("redis");
                return ConnectionMultiplexer.Connect(Connection);

            });
            builder.Services.AddAplicationServices();

            builder.Services.AddScoped(typeof(ITokenService), typeof(TokenService));
            builder.Services.AddIdentity<AppUser, IdentityRole>( options =>
            {
                //options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequireDigit = false;
                //options.Password.RequireLowercase = false;
                //options.Password.RequireUppercase = false;
                //options.Password.RequiredLength = 6;

            }).AddEntityFrameworkStores<AppIdentityDbContext>(); // This will add the Identity services to the DI container
                                                                 // and configure the Identity to use the Entity Framework Core stores.
                                                                 // IUserStore, IUserClaimStore, IUserLoginStore, IUserRoleStore, IUserPasswordStore
            builder.Services.AddAuthentication(
                op =>
                {
                    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
                ).AddJwtBearer(op => 
                {
                    op.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            
            #endregion
            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            


            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {

                var context = services.GetRequiredService<StoreDbContext>();
                await context.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(context);

                var _userManager = services.GetRequiredService<UserManager<AppUser>>();
                var _identityContext = services.GetRequiredService<AppIdentityDbContext>();
                await _identityContext.Database.MigrateAsync();
                await AppIdentityDbContextSeed.SeedAsync(_identityContext);

                
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred during migration");
            }
            
            // Configure the HTTP request pipeline (Middlewares).
            #region Configure
            // app is an instance of WebApplication
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleWare(); 
            };

            app.UseStatusCodePagesWithReExecute("/Errors/{0}"); // This will redirect to the Errors controller and pass the status code as a parameter
            app.UseHttpsRedirection();

            app.UseStaticFiles(); // This will serve static files like images, css, js, etc.
            
            app.UseAuthentication(); // This will authenticate the user
            app.UseAuthorization();

            //app.UseRouting(); // This will route the request to the correct controller
            app.MapControllers();

            #endregion
            app.Run();
        }
    }
}